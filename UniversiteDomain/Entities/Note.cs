namespace UniversiteDomain.Entities;

public class Note
{
	public long IdEtud { get; set; }
	public long IdUe { get; set; }
	public float Valeur { get; set; } = 0.0f;
	public Etudiant? Etudiant { get; set; } = null;
	public Ue? Ue { get; set; } = null;
	
	public override string ToString()
	{
		return "Note de " + Etudiant + " en " + Ue + " : " + Valeur;
	}
}
