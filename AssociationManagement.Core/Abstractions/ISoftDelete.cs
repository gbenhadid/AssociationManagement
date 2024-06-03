namespace AssociationManagement.Core.Common {
    public interface ISoftDelete {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
