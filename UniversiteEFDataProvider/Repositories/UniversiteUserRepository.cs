using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;
using UniversiteEFDataProvider.Entities;

namespace UniversiteEFDataProvider.Repositories;

public class UniversiteUserRepository(UniversiteDbContext context, UserManager<UniversiteUser> userManager, RoleManager<UniversiteRole> roleManager) : Repository<IUniversiteUser>(context), IUniversiteUserRepository
{
    public async Task<IUniversiteUser?> AddUserAsync(string login, string email, string password, string role,  Etudiant? etudiant)
    {
        UniversiteUser user = new UniversiteUser { UserName = login, Email = email, Etudiant = etudiant };
        IdentityResult result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
        await context.SaveChangesAsync();
        return result.Succeeded ? user : null;
    }

    public async Task<IUniversiteUser?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task UpdateAsync(IUniversiteUser entity, string userName, string email)
    {
        UniversiteUser user = (UniversiteUser)entity;

        var existingUser = await context.UniversiteUsers
            .Include(u => u.Etudiant)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (existingUser != null && existingUser.Etudiant != null)
        {
            context.Entry(existingUser.Etudiant).State = EntityState.Detached;
        }

        user.UserName = userName;
        user.Email = email;
        await userManager.UpdateAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<IUniversiteUser?> FindByIdEtudAsync(long etudiantId)
    {
        return await context.UniversiteUsers.FirstOrDefaultAsync(u => u.IdEtud == etudiantId);
    }

    public new async Task<int> DeleteAsync(long id)
    {
        Etudiant etud = await context.Etudiants.FindAsync(id);
        UniversiteUser user = await context.UniversiteUsers.FirstOrDefaultAsync(u => u.IdEtud == id);

        if (user != null)
        {
            await userManager.DeleteAsync(user);
            await context.SaveChangesAsync();
            return 1;
        }
        return 0;
    }

    public async Task<bool> IsInRoleAsync(string email, string role)
    {
        bool res = false;
        var user = await userManager.FindByEmailAsync(email);
        return await userManager.IsInRoleAsync(user, role);
    }

}
