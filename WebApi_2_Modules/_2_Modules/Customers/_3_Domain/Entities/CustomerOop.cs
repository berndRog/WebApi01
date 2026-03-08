namespace WebApi._2_Modules.Customers._3_Domain.Entities;

#region Customer1: class with private fields and public getter/setter methods
public class Customer1 {

   #region--- fields ---------------------------------------------------------------
   private string _firstname = string.Empty;
   private string _lastname = string.Empty;
   private string? _companyName = null;
   private string _email = string.Empty;
   #endregion
   
   #region --- properties Getter/Setter ---------------------------------------------
   public string Firstname {
      get { return _firstname; }
      private set { _firstname = value; }
   }
   public string Lastname {
      get { return _lastname; }
      private set { _lastname = value; }
   }
   public string? CompanyName {
      get { return _companyName; }
      private set { _companyName = value; }
   }
   #endregion
   
   #region--- ctors -----------------------------------------------------------------
   public Customer1() {
   }

   public Customer1(
      string firstname,
      string lastname,
      string? companyName,
      string email
   ) {
      _firstname = firstname;
      _lastname = lastname;
      _companyName = companyName;
      _email = email;
   }   
   #endregion
   
   #region--- methods Getter/Setter --------------------------------------------------
   public string GetFirstname() {
      return _firstname;
   }
   private void SetFirstname(string value) {
      _firstname = value;
   }

   public string GetLastname() {
      return _lastname;
   }
   private void SetLastname(string value) {
      _lastname = value;
   }
   
   public string? GetComponyName() {
      return _companyName;
   }
   private void SetCompanyName(string value) {
      _companyName = value;
   }
   #endregion
   
   #region--- static factory to create a Customer object ----------------------------
   public static Customer1 Create(
      string firstname,
      string lastname,
      string? companyName,
      string email
   ) {
      return new Customer1(
         firstname: firstname,
         lastname: lastname,
         companyName: companyName,
         email: email
      );
   } 
   #endregion
   
   #region--- methods ---------------------------- ------------------------------------
   public string AsString() {
      return 
         $"Vorname: {_firstname}, Nachname: {_lastname}\n" +
         $"Company: {_companyName}\n" +
         $"E-Mail: {_email}";
   }
   #endregion
}
#endregion

#region Customer2: class with private fields and public getter/setter methods using expression-bodied members
public class Customer2 {

   #region--- fields ---------------------------------------------------------------
   private string _firstname = string.Empty;
   private string _lastname = string.Empty;
   private string? _companyName = null;
   private string _email = string.Empty;
   #endregion
   
   #region --- properties Getter/Setter ---------------------------------------------
   public string Firstname {
      get => _firstname; 
      private set =>  _firstname = value; 
   }
   public string Lastname {
      get => _lastname; 
      private set => _lastname = value; 
   }
   public string? CompanyName {
      get => _companyName; 
      private set => _companyName = value; 
   }
   #endregion
   
   #region--- ctors -----------------------------------------------------------------
   private Customer2() {
   }

   private Customer2(
      string firstname,
      string lastname,
      string? companyName,
      string email
   ) {
      _firstname = firstname;
      _lastname = lastname;
      _companyName = companyName;
      _email = email;
   }   
   #endregion
   
   #region--- methods Getter/Setter --------------------------------------------------
   public string GetFirstname() => _firstname;
   private void SetFirstname(string value) => _firstname = value;

   public string GetLastname() => _lastname;
   private void SetLastname(string value) => _lastname = value;
   
   public string? GetComponyName() => _companyName;
   private void SetCompanyName(string value) => _companyName = value;
   #endregion
   
   #region--- static factory to create a Customer object ----------------------------
   public static Customer2 Create(
      string firstname,
      string lastname,
      string? companyName,
      string email
   ) => new Customer2(
         firstname: firstname,
         lastname: lastname,
         companyName: companyName,
         email: email
      );
   #endregion
   
   #region--- methods ---------------------------- ------------------------------------
   public string AsString() 
      => $"Vorname: {_firstname}, Nachname: {_lastname}\n" +
         $"Company: {_companyName}\n" +
         $"E-Mail: {_email}";
   #endregion
   
}
#endregion

#region Customer3: class with auto properties only
public class Customer3 {
   
   #region --- properties Getter/Setter ---------------------------------------------
   // auto-implemented properties with private setter
   public string Firstname { get; private set; } = string.Empty;
   public string Lastname { get; private set; } = string.Empty;
   public string? CompanyName { get; private set; } = null;
   public string Email { get; private set; } = string.Empty;
   #endregion
   
   #region--- ctors -----------------------------------------------------------------
   private Customer3() { }

   private Customer3(
      string firstname,
      string lastname,
      string? companyName,
      string email
   ) {
      Firstname = firstname;
      Lastname = lastname;
      CompanyName = companyName;
      Email = email;
   }   
   #endregion
   
   
   #region--- static factory to create a Customer object ----------------------------
   public static Customer3 Create(
      string firstname,
      string lastname,
      string? companyName,
      string email
   ) => new Customer3(
      firstname: firstname,
      lastname: lastname,
      companyName: companyName,
      email: email
   );
   #endregion
   
   #region--- methods ---------------------------- ------------------------------------
   public string AsString() 
      => $"Vorname: {Firstname}, Nachname: {Lastname}\n" +
         $"Company: {CompanyName}\n" +
         $"E-Mail: {Email}";
   #endregion
}
#endregion

