using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace CustomValidation
{
	/// <summary>
	/// Summary description for ValidationSummaryForm.
	/// </summary>
	public class ValidationSummaryForm : System.Windows.Forms.Form
	{
    private System.Windows.Forms.Button closeButton;
    private System.Windows.Forms.ListBox validationErrorsList;
    private System.Windows.Forms.Label lblErrorMessage;
    private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ValidationSummaryForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.closeButton = new System.Windows.Forms.Button();
      this.validationErrorsList = new System.Windows.Forms.ListBox();
      this.lblErrorMessage = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // closeButton
      // 
      this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.closeButton.Location = new System.Drawing.Point(214, 202);
      this.closeButton.Name = "closeButton";
      this.closeButton.TabIndex = 0;
      this.closeButton.Text = "Close";
      this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
      // 
      // validationErrorsList
      // 
      this.validationErrorsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.validationErrorsList.IntegralHeight = false;
      this.validationErrorsList.Location = new System.Drawing.Point(8, 23);
      this.validationErrorsList.Name = "validationErrorsList";
      this.validationErrorsList.Size = new System.Drawing.Size(280, 153);
      this.validationErrorsList.TabIndex = 1;
      this.validationErrorsList.DoubleClick += new System.EventHandler(this.validationErrorsList_DoubleClick);
      // 
      // lblErrorMessage
      // 
      this.lblErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.lblErrorMessage.Location = new System.Drawing.Point(8, 8);
      this.lblErrorMessage.Name = "lblErrorMessage";
      this.lblErrorMessage.Size = new System.Drawing.Size(280, 16);
      this.lblErrorMessage.TabIndex = 2;
      this.lblErrorMessage.Text = "[ErrorMessage Goes Here]";
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point(8, 176);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(280, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Double-click an error message to edit the related field.";
      // 
      // ValidationSummaryForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(296, 238);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.validationErrorsList);
      this.Controls.Add(this.lblErrorMessage);
      this.Controls.Add(this.closeButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(304, 264);
      this.Name = "ValidationSummaryForm";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "[ErrorCaption Goes Here]";
      this.Load += new System.EventHandler(this.ValidationSummaryForm_Load);
      this.ResumeLayout(false);

    }
		#endregion
    
    public string ErrorCaption {
      get { return this.Text; }
      set { this.Text = value; }
    }
    
    public string ErrorMessage {
      get { return this.lblErrorMessage.Text; }
      set { this.lblErrorMessage.Text = value; }
    }
    
    public void LoadValidators(ValidatorCollection validators) {
    
      // Note: validators should contain all validators under
      // the jurisdiction of the BaseContainerValidator so
      // we can handle both where a control becomes valid and
      // where a control becomes invalid. If we only get 
      // the invalid controls, we can't display controls that were
      // valid at the time but become invalid
      
      // If list currently has items then deregister the 
      // Validate event handler
      if( this.validationErrorsList.Items.Count > 0 ) {
        foreach( BaseValidator validator in this.validationErrorsList.Items ) {
          validator.Validated -= new EventHandler(BaseValidator_Validated);
        }
      }
      
      // Clear the list
      this.validationErrorsList.Items.Clear();
      
      // Add new validators and register the Validate 
      // event handler
      foreach(BaseValidator validator in validators ) {
        if( !validator.IsValid ) {
          this.validationErrorsList.Items.Add(validator);
        }
        validator.Validated += new EventHandler(BaseValidator_Validated);
      }
    }

    private void validationErrorsList_DoubleClick(object sender, System.EventArgs e) {
      // Set focus on the selected BaseValidator's ControlToValidate ie
      // in the owner form
      BaseValidator selected = (BaseValidator)this.validationErrorsList.SelectedItem;
      selected.ControlToValidate.Focus();
    }
    
    public void BaseValidator_Validated(object sender, EventArgs e) {
    
      BaseValidator validator = (BaseValidator)sender;
      
      // If validator is valid, remove from list
      if( validator.IsValid ) {
        this.validationErrorsList.Items.Remove(validator);
      }
      // Add validator to list, in tab index order
      else {
        if( !this.validationErrorsList.Items.Contains(validator) ) {
          decimal tabIndex = validator.FlattenedTabIndex;
          for( int i = 0; i < this.validationErrorsList.Items.Count; i++ ) {
            BaseValidator currentValidator = (BaseValidator)this.validationErrorsList.Items[i];
            if( tabIndex < currentValidator.FlattenedTabIndex ) {
              this.validationErrorsList.Items.Insert(i, validator);
              return;
            }
          }
          this.validationErrorsList.Items.Add(validator);
        }
      }
    }

    private void closeButton_Click(object sender, System.EventArgs e) {
      this.Close();
    }

    private void ValidationSummaryForm_Load(object sender, System.EventArgs e) {
      // Show form to the right of the owner form
      if( this.Owner != null ) {
        this.Top = this.Owner.Top;
        int padding = 10;
        this.Left = this.Owner.Left + this.Owner.Width + padding;
      }
    }
	}
}
