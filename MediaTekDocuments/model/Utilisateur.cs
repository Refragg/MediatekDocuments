namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier représentant un utilisateur de l'application
    /// </summary>
    public class Utilisateur
    {
        public int Id { get; }
        public string Login { get; }
        public bool Administrateur { get; }
        public string Service { get; }

        public Utilisateur(int id, string login, bool administrateur, string service)
        {
            Id = id;
            Login = login;
            Administrateur = administrateur;
            Service = service;
        }
    }
}