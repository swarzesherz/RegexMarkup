using System;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace CustomValidation
{   
  #region ValidationSummaryDisplayMode
  public enum ValidationSummaryDisplayMode {
    List,            // Simple list
    BulletList,      // Bulleted list
    SingleParagraph, // No line-breaks
    Simple,          // Plain MessageBox
  }
  #endregion
  
  #region ValidationDataType
  public enum ValidationDataType 
  {
    Currency,
    Date,
    Double,
    Integer,
    String
  }
  #endregion

  #region ValidationCompareOperator
  public enum ValidationCompareOperator {
    DataTypeCheck,
    Equal,
    GreaterThan,
    GreaterThanEqual,
    LessThan,
    LessThanEqual,
    NotEqual
  }
  #endregion
  
  #region ValidationDepth
  public enum ValidationDepth {
    ContainerOnly,
    All
  }    
  #endregion

  #region ValidatableControlConverter
  public class ValidatableControlConverter : ReferenceConverter {
    public ValidatableControlConverter(Type type) : base(type) { }
      protected override bool IsValueAllowed(ITypeDescriptorContext context, object value) {
      return( (value is TextBox) || 
              (value is ListBox) || 
              (value is ComboBox) || 
              (value is UserControl) );
    }
  }
  #endregion
  
  #region ContainerControlConverter
    
  public class ContainerControlConverter : ReferenceConverter {

    public ContainerControlConverter(Type type) : base(type) {}
            
    protected override bool IsValueAllowed(ITypeDescriptorContext context, object value) {
      return( (value is GroupBox) || 
              (value is TabControl) || 
              (value is Panel) ||
              (value is TabPage) ||
              (value is Form) ||
              (value is UserControl) );
    }
  }
    
  #endregion
  
  #region BaseValidatorComparer
  public class BaseValidatorComparer : IComparer {
    public int Compare(object x, object y) {
      BaseValidator xBaseValidator = (BaseValidator)x;
      BaseValidator yBaseValidator = (BaseValidator)y;
      if( xBaseValidator.FlattenedTabIndex < yBaseValidator.FlattenedTabIndex ) return -1;
      if( xBaseValidator.FlattenedTabIndex > yBaseValidator.FlattenedTabIndex ) return 1;
      return 0;
    }
  }
  #endregion
  
  #region BaseValidator
  public abstract class BaseValidator : Component, ISupportInitialize {

    private Control _controlToValidate = null;
    private string _errorMessage = "";
    private Icon _icon = new Icon(typeof(ErrorProvider), "Error.ico");
    private ErrorProvider _errorProvider = new ErrorProvider();
    private bool _isValid = false;
    private bool _validateOnLoad = false;
    private string _flattenedTabIndex = null;

    #region ISupportInitialize

    public void BeginInit() {}
    public void EndInit() {
      // Hook up ControlToValidate's parent form's Load and Closed events 
      // to register and unregister with the ValidationManager
      // ONLY if _controlToValidate exists at run-time and has a parent form
      // ie has been added to a Form's Controls collection
      // NOTE: if there is no form, we don't add this instance to the ValidatorManager
      // so it is not available for form-wide validation which makes sense
      // since there is no form and therefore no form scope.
      
      if( DesignMode ) return;
      
      Control topMostParent = _controlToValidate;
      while( topMostParent.Parent != null ) {
        topMostParent = topMostParent.Parent;
      }
      if( topMostParent is Form ) {
        ((Form)topMostParent).Load += new EventHandler(Host_Load);
        return;
      }
      if( topMostParent is UserControl ) {
        ((UserControl)topMostParent).Load += new EventHandler(Host_Load);
        return;
      }
//      if( _controlToValidate.Parent is UserControl ) {
//        UserControl userControl = _controlToValidate.Parent as UserControl;
//        userControl.Load += new EventHandler(Form_Load);
//        userControl.Disposed += new EventHandler(Form_Closed);
//        return;
//      }
//
//      Form host = _controlToValidate.FindForm();
//      if( (_controlToValidate != null) && (!DesignMode) && (host != null) ) {
//        host.Load += new EventHandler(Form_Load);
//        host.Closed += new EventHandler(Form_Closed);
//      }
    }

    #endregion
    
    [Category("Appearance")]
    [Description("Sets or returns the text for the error message.")]
    [DefaultValue("")]
    public string ErrorMessage {
      get { return _errorMessage; }
      set { _errorMessage = value; }
    }

    [Category("Appearance")]
    [Description("Sets or returns the Icon to display ErrorMessage.")]
    public Icon Icon {
      get { return _icon; }
      set { _icon = value; }
    }

    [Category("Behavior")]
    [Description("Sets or returns the input control to validate.")]
    [DefaultValue(null)]
    [TypeConverter(typeof(ValidatableControlConverter))]
    public Control ControlToValidate {
      get { return _controlToValidate; }
      set {
        _controlToValidate = value;
      
        // Hook up ControlToValidate’s Validating event at run-time ie not from VS.NET
        if( (_controlToValidate != null) && (!DesignMode) ) {
          _controlToValidate.Validating += new CancelEventHandler(ControlToValidate_Validating);
        }
      }
    }
    
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsValid {
      get { return _isValid; }
      set { _isValid = value; }
    }  
    
    [Category("Behavior")]
    [Description("Sets or returns whether this validator will validate itself when its host form loads.")]
    [DefaultValue(false)]
    public bool ValidateOnLoad {
      get { return _validateOnLoad; }
      set { _validateOnLoad = value; }
    } 
    
    public void Validate() {
    
      // Validate control
      _isValid = this.EvaluateIsValid();

      // Display an error if ControlToValidate is invalid
      string errorMessage = "";
      if( !_isValid ) {
        errorMessage = _errorMessage;
        _errorProvider.Icon = _icon;
      }
      _errorProvider.SetError(_controlToValidate, errorMessage);
      
      OnValidated(new EventArgs());
    }

    public override string ToString() {
      return _errorMessage;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public decimal FlattenedTabIndex {
      get {
        // Generate unique tab index and store it if 
        // not already generated
        if( _flattenedTabIndex == null ) {
          StringBuilder sb = new StringBuilder();
          Control current = _controlToValidate;
          while( current != null ) {
            string tabIndex = current.TabIndex.ToString();
            sb.Insert(0, tabIndex);
            current = current.Parent;
          }
          sb.Insert(0, "0.");
          _flattenedTabIndex = sb.ToString();
        }
        // Return unique tab index
        return decimal.Parse(_flattenedTabIndex);
      }
    }
    
    public event EventHandler Validated;
    protected void OnValidated(EventArgs e) {
      if( Validated != null ) {
        Validated(this, e);
      }
    }
    
    protected abstract bool EvaluateIsValid();
    
    private void ControlToValidate_Validating(object sender, CancelEventArgs e) {
      // We don't cancel if invalid since we don't want to force
      // the focus to remain on ControlToValidate if invalid
      Validate();
    }
    
    private void Host_Load(object sender, EventArgs e) {
      // Register with ValidatorManager
      Form hostingForm = ((Control)sender).FindForm();
      ValidatorManager.Register(this, hostingForm);
      if( _validateOnLoad ) this.Validate();
      hostingForm.Closed += new EventHandler(HostingForm_Closed);
      
    }  
    private void HostingForm_Closed(object sender, EventArgs e) {
      // DeRegister from ValidatorCollection
      Form hostingForm =(Form)sender;
      hostingForm.Closed -= new EventHandler(HostingForm_Closed);
      ValidatorManager.DeRegister(this, (Form)sender);
    }
  }
  #endregion
  
  #region RequiredFieldValidator
  [ToolboxBitmap(typeof(RequiredFieldValidator), "RequiredFieldValidator.ico")]
	public class RequiredFieldValidator : BaseValidator {
	  private string _initialValue = null;
    [Category("Behavior")]
    [DefaultValue(null)]
    [Description("Sets or returns the base value for the validator. The default value is null.")]
    public string InitialValue {
      get { return _initialValue; }
      set { _initialValue = value; }
    }
    protected override bool EvaluateIsValid() {
      string controlValue = ControlToValidate.Text.Trim();
      string initialValue;
      if( _initialValue == null ) initialValue = "";
      else initialValue = _initialValue.Trim();
      return (controlValue != initialValue);
    }
	}
	#endregion
	
  #region RegularExpressionValidator
  [ToolboxBitmap(typeof(RegularExpressionValidator), "RegularExpressionValidator.ico")]
  public class RegularExpressionValidator : BaseValidator {
  
    private string _validationExpression = "";
    
    [Category("Behavior")]
    [Description("Sets or returns the regular expression that determines the pattern used to validate a field.")]
    [DefaultValue("")]
    public string ValidationExpression {
      get { return _validationExpression; }
      set { _validationExpression = value; }
    }

    protected override bool EvaluateIsValid() {
      // Don't validate if empty
      if( ControlToValidate.Text.Trim() == "" ) return true;
      // Successful if match matches the entire text of ControlToValidate
      string input = ControlToValidate.Text.Trim();
      return Regex.IsMatch(input, _validationExpression.Trim());
    }
  }
  #endregion
  
  #region CustomValidator
  [ToolboxBitmap(typeof(CustomValidator), "CustomValidator.ico")]
  [DefaultEvent("Validating")]
  public class CustomValidator : BaseValidator {
    public class ValidatingCancelEventArgs {
      private bool _valid;
      private Control _controlToValidate;
      public ValidatingCancelEventArgs(bool valid, Control controlToValidate) {
        _valid = valid;
        _controlToValidate = controlToValidate;
      }
      public bool Valid {
        get { return _valid; }
        set { _valid = value; }
      }
      public Control ControlToValidate {
        get { return _controlToValidate; }
        set { _controlToValidate = value; }
      }
    }
    public delegate void ValidatingEventHandler(object sender, ValidatingCancelEventArgs e);
    [Category("Action")]
    [Description("Occurs when the CustomValidator validates the value of the ControlToValidate property.")]
    public event ValidatingEventHandler Validating;
    public void OnValidating(ValidatingCancelEventArgs e) {
      if( Validating != null ) Validating(this, e);
    }

    protected override bool EvaluateIsValid() {
      // Pass validation processing to event handler and wait for response
      ValidatingCancelEventArgs args = new ValidatingCancelEventArgs(false, this.ControlToValidate);
      OnValidating(args);
      return args.Valid;
    }
  }
  
  #endregion
  
  #region BaseCompareValidator
  public abstract class BaseCompareValidator : BaseValidator { 
  
    private ValidationDataType   _type = ValidationDataType.String;
    private string[]             _typeTable = new string[5] {"System.Decimal", 
                                                             "System.DateTime",
                                                             "System.Double",
                                                             "System.Int32",
                                                             "System.String"};
                                                             
    [Category("Behavior")]
    [Description("Sets or returns the data type that specifies how to interpret the values being compared.")]
    [DefaultValue(ValidationDataType.String)]
    public ValidationDataType Type {
      get { return _type; }
      set { _type = value; }
    }

    protected TypeConverter TypeConverter {
      get { return TypeDescriptor.GetConverter(System.Type.GetType(_typeTable[(int)_type])); }
    }
      
    protected bool CanConvert(string value) {
      try {
        TypeConverter   _converter = TypeDescriptor.GetConverter(System.Type.GetType(_typeTable[(int)_type]));
        _converter.ConvertFrom(value);
        return true;
      }
      catch { return false; }
    }

    protected string Format(string value) {
      // If currency
      if( _type == ValidationDataType.Currency ) {
          // Convert to decimal format ie remove currency formatting characters
        return Regex.Replace(value, "[$ .]", "");
      }    
      return value;
    }
  }
  #endregion

  #region RangeValidator
  [ToolboxBitmap(typeof(RangeValidator), "RangeValidator.ico")]
  public class RangeValidator : BaseCompareValidator {
  
    private string _minimumValue = "";
    private string _maximumValue = "";
    
    [Category("Behavior")]
    [Description("Sets or returns the value of the control that you are validating, which must be greater than or equal to the value of this property. The default value is an empty string (\"\").")]
    [DefaultValue("")]
    public string MinimumValue {
      get { return _minimumValue; }
      set { _minimumValue = value; }
    }

    [Category("Behavior")]
    [Description("Sets or returns the value of the control that you are validating, which must be less than or equal to the value of this property. The default value is an empty string (\"\").")]
    [DefaultValue("")]
    public string MaximumValue {
      get { return _maximumValue; }
      set { _maximumValue = value; }
    }

    protected override bool EvaluateIsValid() {
      // Don't validate if empty, unless required
      if( ControlToValidate.Text.Trim() == "" ) return true;

      // Validate and convert Minimum
      if( _minimumValue.Trim() == "" ) throw new Exception("MinimumValue must be provided.");
      string formattedMinimumValue = Format(_minimumValue.Trim());
      if( !CanConvert(formattedMinimumValue) ) throw new Exception("MinimumValue cannot be converted to the specified Type.");
      object minimum = TypeConverter.ConvertFrom(formattedMinimumValue);

      // Validate and convert Maximum
      if( _maximumValue.Trim() == "" ) throw new Exception("MaximumValue must be provided.");
      string formattedMaximumValue = Format(_maximumValue.Trim());
      if( !CanConvert(formattedMaximumValue) ) throw new Exception("MaximumValue cannot be converted to the specified Type.");
      object maximum = TypeConverter.ConvertFrom(formattedMaximumValue);

      // Check minimum <= maximum
      if( Comparer.Default.Compare(minimum, maximum) > 0 ) throw new Exception("MinimumValue cannot be greater than MaximumValue.");

      // Check and convert ControlToValue
      string formattedValue = Format(ControlToValidate.Text.Trim());
      if( !CanConvert(formattedValue) ) return false;
      object value = TypeConverter.ConvertFrom(formattedValue);

      // Validate value's range (minimum <= value <= maximum)
      return( (Comparer.Default.Compare(minimum, value) <= 0) && 
        (Comparer.Default.Compare(value, maximum) <= 0) );
    }
  }
  #endregion
  
  #region CompareValidator
  [ToolboxBitmap(typeof(CompareValidator), "CompareValidator.ico")]
  public class CompareValidator : BaseCompareValidator {
  
    private string _valueToCompare = "";
    private Control _controlToCompare = null;
    private ValidationCompareOperator _operator = ValidationCompareOperator.Equal;
    
    [TypeConverter(typeof(ValidatableControlConverter))]
    [Category("Behavior")]
    [Description("Sets or returns the input control to compare with the input control being validated.")]
    [DefaultValue(null)]
    public Control ControlToCompare {
      get { return _controlToCompare; }
      set { _controlToCompare = value; }
    }
    
    [Category("Behavior")]
    [Description("Sets or returns the comparison operation to perform.")]
    [DefaultValue(null)]
    public ValidationCompareOperator Operator {
      get { return _operator; }
      set { _operator = value; }
    }
    
    [Category("Behavior")]
    [Description("Sets or returns a constant value to compare with the value entered by the user into the input control being validated.")]
    [DefaultValue("")]
    public string ValueToCompare {
      get { return _valueToCompare; }
      set { _valueToCompare = value; }
    }
    
    protected override bool EvaluateIsValid() {
      // Don't validate if empty, unless required
      if( ControlToValidate.Text.Trim() == "" ) return true;

      // Can't evaluate if missing ControlToCompare and ValueToCompare
      if( (_controlToCompare == null) && (_valueToCompare == "") ) throw new Exception("The ControlToCompare property cannot be blank.");
      
      // Validate and convert CompareFrom
      string formattedCompareFrom = Format(ControlToValidate.Text);
      bool canConvertFrom = CanConvert(formattedCompareFrom);
      if( canConvertFrom ) {
        if( _operator == ValidationCompareOperator.DataTypeCheck ) return canConvertFrom;
      }
      else return false;
      object compareFrom = TypeConverter.ConvertFrom(formattedCompareFrom);

      // Validate and convert CompareTo
      string formattedCompareTo = Format(((_controlToCompare != null) ? _controlToCompare.Text : _valueToCompare));
      if( !CanConvert(formattedCompareTo) ) throw new Exception("The value you are comparing to cannot be converted to the specified Type.");
      object compareTo = TypeConverter.ConvertFrom(formattedCompareTo);

      // Perform comparison eg ==, >, >=, <, <=, !=
      int result = Comparer.Default.Compare(compareFrom, compareTo);
      switch( _operator ) {
        case ValidationCompareOperator.Equal :
          return (result == 0);
        case ValidationCompareOperator.GreaterThan :
          return (result > 0);
        case ValidationCompareOperator.GreaterThanEqual :
          return (result >= 0);
        case ValidationCompareOperator.LessThan :
          return (result < 0);
        case ValidationCompareOperator.LessThanEqual :
          return (result <= 0);
        case ValidationCompareOperator.NotEqual :
          return ((result != 0));
        default :
          return false;
      }
    }
  }
  #endregion
  
  #region ValidatorCollection (care of http://sellsbrothers.com/tools/#collectiongen)
  /// <summary>
  ///        A strongly-typed collection of <see cref="BaseValidator"/> objects.
  /// </summary>
  [Serializable]
  public class ValidatorCollection : ICollection, IList, IEnumerable, ICloneable {
    #region Interfaces
    /// <summary>
    ///        Supports type-safe iteration over a <see cref="ValidatorCollection"/>.
    /// </summary>
    public interface BaseValidatorCollectionEnumerator {
      /// <summary>
      ///        Gets the current element in the collection.
      /// </summary>
      BaseValidator Current {get;}

      /// <summary>
      ///        Advances the enumerator to the next element in the collection.
      /// </summary>
      /// <exception cref="InvalidOperationException">
      ///        The collection was modified after the enumerator was created.
      /// </exception>
      /// <returns>
      ///        <c>true</c> if the enumerator was successfully advanced to the next element; 
      ///        <c>false</c> if the enumerator has passed the end of the collection.
      /// </returns>
      bool MoveNext();

      /// <summary>
      ///        Sets the enumerator to its initial position, before the first element in the collection.
      /// </summary>
      void Reset();
    }
    #endregion

    private const int DEFAULT_CAPACITY = 16;

    #region Implementation (data)
    private BaseValidator[] m_array;
    private int m_count = 0;
    [NonSerialized]
    private int m_version = 0;
    #endregion
    
    #region Static Wrappers
    /// <summary>
    ///        Creates a synchronized (thread-safe) wrapper for a 
    ///     <c>ValidatorCollection</c> instance.
    /// </summary>
    /// <returns>
    ///     An <c>ValidatorCollection</c> wrapper that is synchronized (thread-safe).
    /// </returns>
    public static ValidatorCollection Synchronized(ValidatorCollection list) {
      if(list==null)
        throw new ArgumentNullException("list");
      return new SyncValidatorCollection(list);
    }
        
    /// <summary>
    ///        Creates a read-only wrapper for a 
    ///     <c>ValidatorCollection</c> instance.
    /// </summary>
    /// <returns>
    ///     An <c>ValidatorCollection</c> wrapper that is read-only.
    /// </returns>
    public static ValidatorCollection ReadOnly(ValidatorCollection list) {
      if(list==null)
        throw new ArgumentNullException("list");
      return new ReadOnlyValidatorCollection(list);
    }
    #endregion

    #region Construction
    /// <summary>
    ///        Initializes a new instance of the <c>ValidatorCollection</c> class
    ///        that is empty and has the default initial capacity.
    /// </summary>
    public ValidatorCollection() {
      m_array = new BaseValidator[DEFAULT_CAPACITY];
    }
        
    /// <summary>
    ///        Initializes a new instance of the <c>ValidatorCollection</c> class
    ///        that has the specified initial capacity.
    /// </summary>
    /// <param name="capacity">
    ///        The number of elements that the new <c>ValidatorCollection</c> is initially capable of storing.
    ///    </param>
    public ValidatorCollection(int capacity) {
      m_array = new BaseValidator[capacity];
    }

    /// <summary>
    ///        Initializes a new instance of the <c>ValidatorCollection</c> class
    ///        that contains elements copied from the specified <c>ValidatorCollection</c>.
    /// </summary>
    /// <param name="c">The <c>ValidatorCollection</c> whose elements are copied to the new collection.</param>
    public ValidatorCollection(ValidatorCollection c) {
      m_array = new BaseValidator[c.Count];
      AddRange(c);
    }

    /// <summary>
    ///        Initializes a new instance of the <c>ValidatorCollection</c> class
    ///        that contains elements copied from the specified <see cref="BaseValidator"/> array.
    /// </summary>
    /// <param name="a">The <see cref="BaseValidator"/> array whose elements are copied to the new list.</param>
    public ValidatorCollection(BaseValidator[] a) {
      m_array = new BaseValidator[a.Length];
      AddRange(a);
    }
    #endregion
        
    #region Operations (type-safe ICollection)
    /// <summary>
    ///        Gets the number of elements actually contained in the <c>ValidatorCollection</c>.
    /// </summary>
    public virtual int Count {
      get { return m_count; }
    }

    /// <summary>
    ///        Copies the entire <c>ValidatorCollection</c> to a one-dimensional
    ///        string array.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="BaseValidator"/> array to copy to.</param>
    public virtual void CopyTo(BaseValidator[] array) {
      this.CopyTo(array, 0);
    }

    /// <summary>
    ///        Copies the entire <c>ValidatorCollection</c> to a one-dimensional
    ///        <see cref="BaseValidator"/> array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="BaseValidator"/> array to copy to.</param>
    /// <param name="start">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    public virtual void CopyTo(BaseValidator[] array, int start) {
      if (m_count > array.GetUpperBound(0) + 1 - start)
        throw new System.ArgumentException("Destination array was not long enough.");
            
      Array.Copy(m_array, 0, array, start, m_count); 
    }

    /// <summary>
    ///        Gets a value indicating whether access to the collection is synchronized (thread-safe).
    /// </summary>
    /// <returns>true if access to the ICollection is synchronized (thread-safe); otherwise, false.</returns>
    public virtual bool IsSynchronized {
      get { return m_array.IsSynchronized; }
    }

    /// <summary>
    ///        Gets an object that can be used to synchronize access to the collection.
    /// </summary>
    public virtual object SyncRoot {
      get { return m_array.SyncRoot; }
    }
    #endregion
        
    #region Operations (type-safe IList)
    /// <summary>
    ///        Gets or sets the <see cref="BaseValidator"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///        <para><paramref name="index"/> is less than zero</para>
    ///        <para>-or-</para>
    ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollection.Count"/>.</para>
    /// </exception>
    public virtual BaseValidator this[int index] {
      get {
        ValidateIndex(index); // throws
        return m_array[index]; 
      }
      set {
        ValidateIndex(index); // throws
        ++m_version; 
        m_array[index] = value; 
      }
    }

    /// <summary>
    ///        Adds a <see cref="BaseValidator"/> to the end of the <c>ValidatorCollection</c>.
    /// </summary>
    /// <param name="item">The <see cref="BaseValidator"/> to be added to the end of the <c>ValidatorCollection</c>.</param>
    /// <returns>The index at which the value has been added.</returns>
    public virtual int Add(BaseValidator item) {
      if (m_count == m_array.Length)
        EnsureCapacity(m_count + 1);

      m_array[m_count] = item;
      m_version++;

      return m_count++;
    }
        
    /// <summary>
    ///        Removes all elements from the <c>ValidatorCollection</c>.
    /// </summary>
    public virtual void Clear() {
      ++m_version;
      m_array = new BaseValidator[DEFAULT_CAPACITY];
      m_count = 0;
    }
        
    /// <summary>
    ///        Creates a shallow copy of the <see cref="ValidatorCollection"/>.
    /// </summary>
    public virtual object Clone() {
      ValidatorCollection newColl = new ValidatorCollection(m_count);
      Array.Copy(m_array, 0, newColl.m_array, 0, m_count);
      newColl.m_count = m_count;
      newColl.m_version = m_version;

      return newColl;
    }

    /// <summary>
    ///        Determines whether a given <see cref="BaseValidator"/> is in the <c>ValidatorCollection</c>.
    /// </summary>
    /// <param name="item">The <see cref="BaseValidator"/> to check for.</param>
    /// <returns><c>true</c> if <paramref name="item"/> is found in the <c>ValidatorCollection</c>; otherwise, <c>false</c>.</returns>
    public virtual bool Contains(BaseValidator item) {
      for (int i=0; i != m_count; ++i)
        if (m_array[i].Equals(item))
          return true;
      return false;
    }

    /// <summary>
    ///        Returns the zero-based index of the first occurrence of a <see cref="BaseValidator"/>
    ///        in the <c>ValidatorCollection</c>.
    /// </summary>
    /// <param name="item">The <see cref="BaseValidator"/> to locate in the <c>ValidatorCollection</c>.</param>
    /// <returns>
    ///        The zero-based index of the first occurrence of <paramref name="item"/> 
    ///        in the entire <c>ValidatorCollection</c>, if found; otherwise, -1.
    ///    </returns>
    public virtual int IndexOf(BaseValidator item) {
      for (int i=0; i != m_count; ++i)
        if (m_array[i].Equals(item))
          return i;
      return -1;
    }

    /// <summary>
    ///        Inserts an element into the <c>ValidatorCollection</c> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
    /// <param name="item">The <see cref="BaseValidator"/> to insert.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///        <para><paramref name="index"/> is less than zero</para>
    ///        <para>-or-</para>
    ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollection.Count"/>.</para>
    /// </exception>
    public virtual void Insert(int index, BaseValidator item) {
      ValidateIndex(index, true); // throws
            
      if (m_count == m_array.Length)
        EnsureCapacity(m_count + 1);

      if (index < m_count) {
        Array.Copy(m_array, index, m_array, index + 1, m_count - index);
      }

      m_array[index] = item;
      m_count++;
      m_version++;
    }

    /// <summary>
    ///        Removes the first occurrence of a specific <see cref="BaseValidator"/> from the <c>ValidatorCollection</c>.
    /// </summary>
    /// <param name="item">The <see cref="BaseValidator"/> to remove from the <c>ValidatorCollection</c>.</param>
    /// <exception cref="ArgumentException">
    ///        The specified <see cref="BaseValidator"/> was not found in the <c>ValidatorCollection</c>.
    /// </exception>
    public virtual void Remove(BaseValidator item) {           
      int i = IndexOf(item);
      if (i < 0)
        throw new System.ArgumentException("Cannot remove the specified item because it was not found in the specified Collection.");
            
      ++m_version;
      RemoveAt(i);
    }

    /// <summary>
    ///        Removes the element at the specified index of the <c>ValidatorCollection</c>.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///        <para><paramref name="index"/> is less than zero</para>
    ///        <para>-or-</para>
    ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollection.Count"/>.</para>
    /// </exception>
    public virtual void RemoveAt(int index) {
      ValidateIndex(index); // throws
            
      m_count--;

      if (index < m_count) {
        Array.Copy(m_array, index + 1, m_array, index, m_count - index);
      }
            
      // We can't set the deleted entry equal to null, because it might be a value type.
      // Instead, we'll create an empty single-element array of the right type and copy it 
      // over the entry we want to erase.
      BaseValidator[] temp = new BaseValidator[1];
      Array.Copy(temp, 0, m_array, m_count, 1);
      m_version++;
    }

    /// <summary>
    ///        Gets a value indicating whether the collection has a fixed size.
    /// </summary>
    /// <value>true if the collection has a fixed size; otherwise, false. The default is false</value>
    public virtual bool IsFixedSize {
      get { return false; }
    }

    /// <summary>
    ///        gets a value indicating whether the IList is read-only.
    /// </summary>
    /// <value>true if the collection is read-only; otherwise, false. The default is false</value>
    public virtual bool IsReadOnly {
      get { return false; }
    }
    #endregion

    #region Operations (type-safe IEnumerable)
        
    /// <summary>
    ///        Returns an enumerator that can iterate through the <c>ValidatorCollection</c>.
    /// </summary>
    /// <returns>An <see cref="Enumerator"/> for the entire <c>ValidatorCollection</c>.</returns>
    public virtual BaseValidatorCollectionEnumerator GetEnumerator() {
      return new Enumerator(this);
    }
    #endregion

    #region Public helpers (just to mimic some nice features of ArrayList)
        
    /// <summary>
    ///        Gets or sets the number of elements the <c>ValidatorCollection</c> can contain.
    /// </summary>
    public virtual int Capacity {
      get { return m_array.Length; }
            
      set {
        if (value < m_count)
          value = m_count;

        if (value != m_array.Length) {
          if (value > 0) {
            BaseValidator[] temp = new BaseValidator[value];
            Array.Copy(m_array, temp, m_count);
            m_array = temp;
          }
          else {
            m_array = new BaseValidator[DEFAULT_CAPACITY];
          }
        }
      }
    }

    /// <summary>
    ///        Adds the elements of another <c>ValidatorCollection</c> to the current <c>ValidatorCollection</c>.
    /// </summary>
    /// <param name="x">The <c>ValidatorCollection</c> whose elements should be added to the end of the current <c>ValidatorCollection</c>.</param>
    /// <returns>The new <see cref="ValidatorCollection.Count"/> of the <c>ValidatorCollection</c>.</returns>
    public virtual int AddRange(ValidatorCollection x) {
      if (m_count + x.Count >= m_array.Length)
        EnsureCapacity(m_count + x.Count);
            
      Array.Copy(x.m_array, 0, m_array, m_count, x.Count);
      m_count += x.Count;
      m_version++;

      return m_count;
    }

    /// <summary>
    ///        Adds the elements of a <see cref="BaseValidator"/> array to the current <c>ValidatorCollection</c>.
    /// </summary>
    /// <param name="x">The <see cref="BaseValidator"/> array whose elements should be added to the end of the <c>ValidatorCollection</c>.</param>
    /// <returns>The new <see cref="ValidatorCollection.Count"/> of the <c>ValidatorCollection</c>.</returns>
    public virtual int AddRange(BaseValidator[] x) {
      if (m_count + x.Length >= m_array.Length)
        EnsureCapacity(m_count + x.Length);

      Array.Copy(x, 0, m_array, m_count, x.Length);
      m_count += x.Length;
      m_version++;

      return m_count;
    }
        
    /// <summary>
    ///        Sets the capacity to the actual number of elements.
    /// </summary>
    public virtual void TrimToSize() {
      this.Capacity = m_count;
    }

    #endregion

    #region Implementation (helpers)

    /// <exception cref="ArgumentOutOfRangeException">
    ///        <para><paramref name="index"/> is less than zero</para>
    ///        <para>-or-</para>
    ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollection.Count"/>.</para>
    /// </exception>
    private void ValidateIndex(int i) {
      ValidateIndex(i, false);
    }

    /// <exception cref="ArgumentOutOfRangeException">
    ///        <para><paramref name="index"/> is less than zero</para>
    ///        <para>-or-</para>
    ///        <para><paramref name="index"/> is equal to or greater than <see cref="ValidatorCollection.Count"/>.</para>
    /// </exception>
    private void ValidateIndex(int i, bool allowEqualEnd) {
      int max = (allowEqualEnd)?(m_count):(m_count-1);
      if (i < 0 || i > max)
        throw new System.ArgumentOutOfRangeException("Index was out of range.  Must be non-negative and less than the size of the collection.", (object)i, "Specified argument was out of the range of valid values.");
    }

    private void EnsureCapacity(int min) {
      int newCapacity = ((m_array.Length == 0) ? DEFAULT_CAPACITY : m_array.Length * 2);
      if (newCapacity < min)
        newCapacity = min;

      this.Capacity = newCapacity;
    }

    #endregion
        
    #region Implementation (ICollection)

    void ICollection.CopyTo(Array array, int start) {
      this.CopyTo((BaseValidator[])array, start);
    }

    #endregion

    #region Implementation (IList)

    object IList.this[int i] {
      get { return (object)this[i]; }
      set { this[i] = (BaseValidator)value; }
    }

    int IList.Add(object x) {
      return this.Add((BaseValidator)x);
    }

    bool IList.Contains(object x) {
      return this.Contains((BaseValidator)x);
    }

    int IList.IndexOf(object x) {
      return this.IndexOf((BaseValidator)x);
    }

    void IList.Insert(int pos, object x) {
      this.Insert(pos, (BaseValidator)x);
    }

    void IList.Remove(object x) {
      this.Remove((BaseValidator)x);
    }

    void IList.RemoveAt(int pos) {
      this.RemoveAt(pos);
    }

    #endregion

    #region Implementation (IEnumerable)

    IEnumerator IEnumerable.GetEnumerator() {
      return (IEnumerator)(this.GetEnumerator());
    }

    #endregion

    #region Nested enumerator class
    /// <summary>
    ///        Supports simple iteration over a <see cref="ValidatorCollection"/>.
    /// </summary>
    private class Enumerator : IEnumerator, BaseValidatorCollectionEnumerator {
      #region Implementation (data)
            
      private ValidatorCollection m_collection;
      private int m_index;
      private int m_version;
            
      #endregion
        
      #region Construction
            
      /// <summary>
      ///        Initializes a new instance of the <c>Enumerator</c> class.
      /// </summary>
      /// <param name="tc"></param>
      internal Enumerator(ValidatorCollection tc) {
        m_collection = tc;
        m_index = -1;
        m_version = tc.m_version;
      }
            
      #endregion
    
      #region Operations (type-safe IEnumerator)
            
      /// <summary>
      ///        Gets the current element in the collection.
      /// </summary>
      public BaseValidator Current {
        get { return m_collection[m_index]; }
      }

      /// <summary>
      ///        Advances the enumerator to the next element in the collection.
      /// </summary>
      /// <exception cref="InvalidOperationException">
      ///        The collection was modified after the enumerator was created.
      /// </exception>
      /// <returns>
      ///        <c>true</c> if the enumerator was successfully advanced to the next element; 
      ///        <c>false</c> if the enumerator has passed the end of the collection.
      /// </returns>
      public bool MoveNext() {
        if (m_version != m_collection.m_version)
          throw new System.InvalidOperationException("Collection was modified; enumeration operation may not execute.");

        ++m_index;
        return (m_index < m_collection.Count) ? true : false;
      }

      /// <summary>
      ///        Sets the enumerator to its initial position, before the first element in the collection.
      /// </summary>
      public void Reset() {
        m_index = -1;
      }
      #endregion
    
      #region Implementation (IEnumerator)
            
      object IEnumerator.Current {
        get { return (object)(this.Current); }
      }
            
      #endregion
    }
    #endregion
        
    #region Nested Syncronized Wrapper class
    private class SyncValidatorCollection : ValidatorCollection {
      #region Implementation (data)
      private ValidatorCollection m_collection;
      private object m_root;
      #endregion

      #region Construction
      internal SyncValidatorCollection(ValidatorCollection list) {
        m_root = list.SyncRoot;
        m_collection = list;
      }
      #endregion
            
      #region Type-safe ICollection
      public override void CopyTo(BaseValidator[] array) {
        lock(this.m_root)
          m_collection.CopyTo(array);
      }

      public override void CopyTo(BaseValidator[] array, int start) {
        lock(this.m_root)
          m_collection.CopyTo(array,start);
      }
      public override int Count {
        get { 
          lock(this.m_root)
            return m_collection.Count;
        }
      }

      public override bool IsSynchronized {
        get { return true; }
      }

      public override object SyncRoot {
        get { return this.m_root; }
      }
      #endregion
            
      #region Type-safe IList
      public override BaseValidator this[int i] {
        get {
          lock(this.m_root)
            return m_collection[i];
        }
        set {
          lock(this.m_root)
            m_collection[i] = value; 
        }
      }

      public override int Add(BaseValidator x) {
        lock(this.m_root)
          return m_collection.Add(x);
      }
            
      public override void Clear() {
        lock(this.m_root)
          m_collection.Clear();
      }

      public override bool Contains(BaseValidator x) {
        lock(this.m_root)
          return m_collection.Contains(x);
      }

      public override int IndexOf(BaseValidator x) {
        lock(this.m_root)
          return m_collection.IndexOf(x);
      }

      public override void Insert(int pos, BaseValidator x) {
        lock(this.m_root)
          m_collection.Insert(pos,x);
      }

      public override void Remove(BaseValidator x) {           
        lock(this.m_root)
          m_collection.Remove(x);
      }

      public override void RemoveAt(int pos) {
        lock(this.m_root)
          m_collection.RemoveAt(pos);
      }
            
      public override bool IsFixedSize {
        get {return m_collection.IsFixedSize;}
      }

      public override bool IsReadOnly {
        get {return m_collection.IsReadOnly;}
      }
      #endregion

      #region Type-safe IEnumerable
      public override BaseValidatorCollectionEnumerator GetEnumerator() {
        lock(m_root)
          return m_collection.GetEnumerator();
      }
      #endregion

      #region Public Helpers
      // (just to mimic some nice features of ArrayList)
      public override int Capacity {
        get {
          lock(this.m_root)
            return m_collection.Capacity;
        }
                
        set {
          lock(this.m_root)
            m_collection.Capacity = value;
        }
      }

      public override int AddRange(ValidatorCollection x) {
        lock(this.m_root)
          return m_collection.AddRange(x);
      }

      public override int AddRange(BaseValidator[] x) {
        lock(this.m_root)
          return m_collection.AddRange(x);
      }
      #endregion
    }
    #endregion

    #region Nested Read Only Wrapper class
    private class ReadOnlyValidatorCollection : ValidatorCollection {
      #region Implementation (data)
      private ValidatorCollection m_collection;
      #endregion

      #region Construction
      internal ReadOnlyValidatorCollection(ValidatorCollection list) {
        m_collection = list;
      }
      #endregion
            
      #region Type-safe ICollection
      public override void CopyTo(BaseValidator[] array) {
        m_collection.CopyTo(array);
      }

      public override void CopyTo(BaseValidator[] array, int start) {
        m_collection.CopyTo(array,start);
      }
      public override int Count {
        get {return m_collection.Count;}
      }

      public override bool IsSynchronized {
        get { return m_collection.IsSynchronized; }
      }

      public override object SyncRoot {
        get { return this.m_collection.SyncRoot; }
      }
      #endregion
            
      #region Type-safe IList
      public override BaseValidator this[int i] {
        get { return m_collection[i]; }
        set { throw new NotSupportedException("This is a Read Only Collection and can not be modified"); }
      }

      public override int Add(BaseValidator x) {
        throw new NotSupportedException("This is a Read Only Collection and can not be modified");
      }
            
      public override void Clear() {
        throw new NotSupportedException("This is a Read Only Collection and can not be modified");
      }

      public override bool Contains(BaseValidator x) {
        return m_collection.Contains(x);
      }

      public override int IndexOf(BaseValidator x) {
        return m_collection.IndexOf(x);
      }

      public override void Insert(int pos, BaseValidator x) {
        throw new NotSupportedException("This is a Read Only Collection and can not be modified");
      }

      public override void Remove(BaseValidator x) {           
        throw new NotSupportedException("This is a Read Only Collection and can not be modified");
      }

      public override void RemoveAt(int pos) {
        throw new NotSupportedException("This is a Read Only Collection and can not be modified");
      }
            
      public override bool IsFixedSize {
        get {return true;}
      }

      public override bool IsReadOnly {
        get {return true;}
      }
      #endregion

      #region Type-safe IEnumerable
      public override BaseValidatorCollectionEnumerator GetEnumerator() {
        return m_collection.GetEnumerator();
      }
      #endregion

      #region Public Helpers
      // (just to mimic some nice features of ArrayList)
      public override int Capacity {
        get { return m_collection.Capacity; }
                
        set { throw new NotSupportedException("This is a Read Only Collection and can not be modified"); }
      }

      public override int AddRange(ValidatorCollection x) {
        throw new NotSupportedException("This is a Read Only Collection and can not be modified");
      }

      public override int AddRange(BaseValidator[] x) {
        throw new NotSupportedException("This is a Read Only Collection and can not be modified");
      }
      #endregion
    }
    #endregion
  }

  #endregion
  
  #region ValidatorManager
    
  public class ValidatorManager {

    private static Hashtable _validators = new Hashtable();

    public static void Register(BaseValidator validator, Form hostingForm) {
      
      // Create form bucket if it doesn't exist
      if( _validators[hostingForm] == null ) {
        _validators[hostingForm] = new ValidatorCollection();
      }
      
      // Add this validator to the list of registered validators
      ValidatorCollection validators = 
        (ValidatorCollection)_validators[hostingForm];
      validators.Add(validator);
    }
    
    public static ValidatorCollection GetValidators(Form hostingForm) {
      return (ValidatorCollection)_validators[hostingForm];
    }
    
    public static ValidatorCollection GetValidators(Form hostingForm, Control container, ValidationDepth validationDepth) {
    
      ValidatorCollection validators = ValidatorManager.GetValidators(hostingForm);
      ValidatorCollection contained = new ValidatorCollection();
      foreach(BaseValidator validator in validators ) {
        // Only validate BaseValidators hosted by the container I reference
        if( IsParent(container, validator.ControlToValidate, validationDepth) ) { 
          contained.Add(validator);
        }
      }
      return contained;
    }
    
    public static void DeRegister(BaseValidator validator, Form hostingForm) {
    
      // Remove this validator from the list of registered validators
      ValidatorCollection validators = (ValidatorCollection)_validators[hostingForm];
      validators.Remove(validator);
      
      // Remove form bucket if all validators on the form are de-registered
      if( validators.Count == 0 ) _validators.Remove(hostingForm);
    }
    
    private static bool IsParent(Control parent, Control child, ValidationDepth validationDepth) {
      if( validationDepth == ValidationDepth.ContainerOnly ) {
        return( child.Parent == parent );
      }
      else {
        Control current = child;
        while( current != null ) {
          if( current == parent ) return true;
    		        
          current = current.Parent;
        }
        return false;
      }
    }
  }
  
  #endregion

  #region SummarizeEventArgs
  public class SummarizeEventArgs {
    public ValidatorCollection Validators;
    public Form HostingForm;
    public SummarizeEventArgs(ValidatorCollection validators, Form hostingForm) {
      Validators = validators;
      HostingForm = hostingForm;
    }
  }
  #endregion
  
  #region SummarizeEventHandler
  public delegate void SummarizeEventHandler(object sender, SummarizeEventArgs e);
  #endregion
  
  #region BaseContainerValidator
  
  public abstract class BaseContainerValidator : Component {

    private Form _hostingForm = null;

    [Browsable(false)]
    [DefaultValue(null)] 
    public Form HostingForm {
      get {
        if( (_hostingForm == null) && DesignMode ) {
          // See if we're being hosted in VS.NET (or something similar)
          // Cheers Ian Griffiths/Chris Sells for this code
          IDesignerHost designer = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
          if( designer != null ) _hostingForm = designer.RootComponent as Form;
        }
        return _hostingForm;
      }
      set {
        if( !DesignMode ) {
          // Only allow this property to be set if:
          //    a) it is being set for the first time
          //    b) it is the same Form as the original form
          if( (_hostingForm != null) && (_hostingForm != value) ) {
            throw new Exception("Can't change HostingForm at runtime.");
          }
          else _hostingForm = value;
        }
      }
    }
    
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsValid {
      get {
        foreach(BaseValidator validator in GetValidators() ) {
          if( !validator.IsValid ) return false;
        }
        return true;
      }
    }

    public void Validate() {            
      // Validate
      BaseValidator firstInTabOrder = null;
      foreach(BaseValidator validator in GetValidators() ) {
      
        // Validate control
        validator.Validate();
                
        // Set focus on the control it its invalid and the earliest invalid
        // control in the tab order
        if( !validator.IsValid ) {
          if( (firstInTabOrder == null) || 
            (firstInTabOrder.FlattenedTabIndex > validator.FlattenedTabIndex) ) {
            firstInTabOrder = validator;
          }
        }
      }

      OnSummarize(new SummarizeEventArgs(GetValidators(), HostingForm));
      
      // Select first invalid control in tab order, if any
      if( firstInTabOrder != null ) {
        firstInTabOrder.ControlToValidate.Focus();
      }
    }

    public abstract ValidatorCollection GetValidators();

    public event SummarizeEventHandler Summarize;
    protected void OnSummarize(SummarizeEventArgs e) {
      if( Summarize != null ) {
        Summarize(this, e);
      }
    }
        
  }
    
  #endregion
  
  #region ContainerValidator
  [ToolboxBitmap(typeof(ContainerValidator), "ContainerValidator.ico")]
  public class ContainerValidator : BaseContainerValidator {
      
    private Control _containerToValidate = null;
    private ValidationDepth _validationDepth  = ValidationDepth.All;

    [Category("Behavior")]
    [Description("Sets or returns the input control to validate.")]
    [DefaultValue(null)]
    [TypeConverter(typeof(ContainerControlConverter))]
    public Control ContainerToValidate {
      get { return _containerToValidate; }
      set { _containerToValidate = value; }
    }
    
    [Category("Behavior")]
    [DefaultValue(ValidationDepth.All)]
    [Description(@"Sets or returns the level of validation applied to ContainerToValidate using the ValidationDepth enumeration.")]
    public ValidationDepth ValidationDepth {
      get { return _validationDepth; }
      set { _validationDepth = value; }
    }
    
    public override ValidatorCollection GetValidators() {
      return ValidatorManager.GetValidators(HostingForm, _containerToValidate, _validationDepth);
    }

  }
  #endregion
  
  #region FormValidator
  [ToolboxBitmap(typeof(FormValidator), "FormValidator.ico")]
  public class FormValidator : BaseContainerValidator, ISupportInitialize {
  
    private bool _validateOnAccept = true;
      
    #region ISupportInitialize

    public void BeginInit() {}

    public void EndInit() {
      // Handle AcceptButton click if requested
      if( (HostingForm != null) && _validateOnAccept ) {
        Button acceptButton = (Button)HostingForm.AcceptButton;
        if( acceptButton != null ) {
          acceptButton.Click += new EventHandler(AcceptButton_Click);
        }
      }
    }

    #endregion

    [Category("Behavior")]
    [Description("If the host form's AcceptButton property is set, automatically validate when it is clicked. The AcceptButton's DialogResult property must be set to 'OK'.")]
    [DefaultValue(true)]
    public bool ValidateOnAccept {
      get { return _validateOnAccept; }
      set { _validateOnAccept = value; }
    }
    
    public override ValidatorCollection GetValidators() {
      return ValidatorManager.GetValidators(HostingForm);
    }

    private void AcceptButton_Click(object sender, System.EventArgs e) {     
      // If DialogResult is OK, that means we need to return None
      if( HostingForm.DialogResult == DialogResult.OK ) {
        Validate();
        if( !IsValid ) {
          HostingForm.DialogResult = DialogResult.None;
        }
      }
    }
  }
  #endregion
  
  #region BaseValidationSummary
  
  [ProvideProperty("ShowSummary", typeof(BaseContainerValidator))]
  [ProvideProperty("ErrorMessage", typeof(BaseContainerValidator))]
  [ProvideProperty("ErrorCaption", typeof(BaseContainerValidator))]
  public abstract class BaseValidationSummary : Component, IExtenderProvider {
  
    private Hashtable _showSummaries = new Hashtable(); 
    private Hashtable _errorMessages = new Hashtable();
    private Hashtable _errorCaptions = new Hashtable();
    
    #region IExtenderProvider
    bool IExtenderProvider.CanExtend(object extendee) {
      return true;
    }
    #endregion

    [Category("Validation Summary")]
    [Description("Sets or returns whether BaseContainerValidator uses the ValidationSummary component.")]
    [DefaultValue(false)]
    public bool GetShowSummary(BaseContainerValidator extendee) {
      if( _showSummaries.Contains(extendee) ) {
        return (bool)_showSummaries[extendee];
      }
      else {
        return false;
      }
    }
    public void SetShowSummary(BaseContainerValidator extendee, bool value) {
      if( value == true ) {
        _showSummaries[extendee] = value;
        extendee.Summarize += new SummarizeEventHandler(Summarize);
      }
      else {
        _showSummaries.Remove(extendee);
      }
    }

    [Category("Validation Summary")]
    [Description("Sets or returns the message to display with the validation summary.")]
    [DefaultValue("")]
    public string GetErrorMessage(BaseContainerValidator extendee) {
      if( _errorMessages.Contains(extendee) ) {
        return (string)_errorMessages[extendee];
      }
      else {
        return "";
      }
    }
    public void SetErrorMessage(BaseContainerValidator extendee, string value) {
      if( value != null ) {
        _errorMessages[extendee] = value;
      }
      else {
        _errorMessages.Remove(extendee);
      }
    }

    [Category("Validation Summary")]
    [Description("Sets or returns the caption to display with the validation summary.")]
    [DefaultValue("")]
    public string GetErrorCaption(BaseContainerValidator extendee) {
      if( _errorCaptions.Contains(extendee) ) {
        return (string)_errorCaptions[extendee];
      }
      else {
        return "";
      }
    }
    public void SetErrorCaption(BaseContainerValidator extendee, string value) {
      if( value != null ) {
        _errorCaptions[extendee] = value;
      }
      else {
        _errorCaptions.Remove(extendee);
      }
    }
    
    
    protected abstract void Summarize(object sender, SummarizeEventArgs e);
    
    // Support validation in flattened tab index order
    protected ValidatorCollection Sort(ValidatorCollection validators) {
    
      // Sort validators into flattened tab index order
      // using the BaseValidatorComparer
      ArrayList sorted = new ArrayList();
      foreach( BaseValidator validator in validators ) {
        sorted.Add(validator);
      }
      sorted.Sort(new BaseValidatorComparer());
      ValidatorCollection sortedValidators = new ValidatorCollection();
      foreach( BaseValidator validator in sorted ) {
        sortedValidators.Add(validator);
      }
      
      return sortedValidators;
    }
  }
  
  #endregion
  
  #region ValidationSummary
  [ToolboxBitmap(typeof(ValidationSummary), "ValidationSummary.ico")]
  [ProvideProperty("DisplayMode", typeof(BaseContainerValidator))]
  public class ValidationSummary : BaseValidationSummary {
  
      private Hashtable _displayModes = new Hashtable(); 
      
    [Category("Validation Summary")]
    [Description("Sets or returns how the validation summary will be displayed.")]
    [DefaultValue(ValidationSummaryDisplayMode.Simple)]
    public ValidationSummaryDisplayMode GetDisplayMode(BaseContainerValidator extendee) {
      if( _displayModes.Contains(extendee) ) {
        return (ValidationSummaryDisplayMode)_displayModes[extendee];
      }
      else {
        return ValidationSummaryDisplayMode.Simple;
      }
    }
    public void SetDisplayMode(BaseContainerValidator extendee, ValidationSummaryDisplayMode value) {
      _displayModes[extendee] = value;
    }

    protected override void Summarize(object sender, SummarizeEventArgs e) {
      
      // Don’t validate if no validators were passed
      if( e.Validators.Count == 0 ) {
        return;
      }
      
      BaseContainerValidator extendee = (BaseContainerValidator)sender;
      ValidationSummaryDisplayMode displayMode = GetDisplayMode(extendee);      
      ValidatorCollection validators = e.Validators;
            
      // Make sure there are validators
      if( (validators == null) || (validators.Count == 0) ) return;

      string errorMessage = GetErrorMessage(extendee);
      string errorCaption = GetErrorCaption(extendee);
     
      // Get error text, if provided
      if( errorMessage == null ) {
        errorMessage = "";
      }
            
      // Get error caption, if provided
      if( errorCaption == null ) {
        errorCaption = "";
      }
      
      // Build summary message body
      string errors = "";
      if( displayMode == ValidationSummaryDisplayMode.Simple ) {
        // Build Simple message
        errors = errorMessage;
      }
      else {
        // Build List, BulletList or SingleParagraph
        foreach(object validator in base.Sort(validators)) {
          BaseValidator current = (BaseValidator)validator;
          if( !current.IsValid ) {
            switch( displayMode ) {
              case ValidationSummaryDisplayMode.List:
                errors += string.Format("{0}\n", current.ErrorMessage);
                break;
              case ValidationSummaryDisplayMode.BulletList:
                errors += string.Format("- {0}\n", current.ErrorMessage);
                break;
              case ValidationSummaryDisplayMode.SingleParagraph:
                errors += string.Format("{0}. ", current.ErrorMessage);
                break;
            }
          }
        }
        // Prepend error message, if provided
        if( (errors != "") && (errorMessage != "") ) {
          errors = string.Format("{0}\n\n{1}", errorMessage.Trim(), errors);
        }
      }

      // Display summary message
      MessageBox.Show(errors, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
  }
  #endregion
  
  #region ListValidationSummary
  [ToolboxBitmap(typeof(ListValidationSummary), "ListValidationSummary.ico")]
  public class ListValidationSummary : BaseValidationSummary {
  
    ValidationSummaryForm _dlg = null;
    BaseContainerValidator currentExtendee = null;
    
    protected override void Summarize(object sender, SummarizeEventArgs e) {
    
      // Close form if open and nothing invalid
      if( e.Validators.Count == 0 ) {
        if( _dlg != null ) {
          _dlg.Close();
          _dlg = null;
          currentExtendee = null;
        }
        return;
      }
      
      BaseContainerValidator extendee = (BaseContainerValidator)sender;
      
      // If the ValidationSummaryForm is open, but refers to a different extendee
      // (BaseContainerValidator), get rid of it
      if( (_dlg != null) && (currentExtendee != null) && (extendee != currentExtendee)  ) {
        _dlg.Close();
        _dlg = null;
        currentExtendee = extendee;
      }
      
      // Open ValidationSummaryForm if it hasn't been opened,
      // or has been closed since Summarize was last called
      if( _dlg == null ) {
        _dlg = new ValidationSummaryForm();
        _dlg.ErrorCaption = GetErrorCaption(extendee);
        _dlg.ErrorMessage = GetErrorMessage(extendee);
        _dlg.Owner = extendee.HostingForm;
        
        // Register Disposed to handle clean up when user closes form
        _dlg.Disposed += new EventHandler(ValidationSummaryForm_Disposed);
      }
      
      // Get complete set of Validators under the jurisdiction
      // of the BaseContainerValidator
      _dlg.LoadValidators(Sort(extendee.GetValidators()));
      
      // Show dialog if not already visible
      if( !_dlg.Visible ) _dlg.Show();
    }
    
    private void ValidationSummaryForm_Disposed(object sender, EventArgs e) {
      // Clean up if user closes form
      _dlg.Disposed -= new EventHandler(ValidationSummaryForm_Disposed);
      _dlg = null;
    }
  }
  #endregion
}