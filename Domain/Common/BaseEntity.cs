namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime CriadaEm { get; private set; }
        public DateTime? AtualizadaEm { get; protected set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CriadaEm = DateTime.UtcNow;
        }
    }
}
