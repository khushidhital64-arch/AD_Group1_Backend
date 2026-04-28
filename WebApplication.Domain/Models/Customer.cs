namespace WebApplications.Domain.Models
{
    public class Customer
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public Users User { get; set; } = null!;

        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
        public ICollection<ServiceAppointment> ServiceAppointments { get; set; } = new List<ServiceAppointment>();
        public ICollection<PartRequest> PartRequests { get; set; } = new List<PartRequest>();
        public ICollection<ServiceReview> ServiceReviews { get; set; } = new List<ServiceReview>();
    }
}