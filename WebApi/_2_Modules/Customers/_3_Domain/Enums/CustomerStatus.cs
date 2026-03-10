namespace WebApi._2_Modules.Customers._3_Domain.Enums;

public enum CustomerStatus {
   Pending = 0,     // Registered, identity not yet verified by an employee
   Active = 1,      // Identity verified and approved by an employee
   Rejected = 2,    // Registration rejected after identity check
   Deactivated = 3  // Customer relationship closed
}
