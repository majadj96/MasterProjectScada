namespace UserInterface.BaseError
{
    public class ValidationBase : BindableBase
    {
        public ValidationErrors ValidationErrors { get; set; }
        private bool isValid;

        public bool IsValid
        {
            get { return isValid; }
        }

        protected ValidationBase()
        {
            this.ValidationErrors = new ValidationErrors();
        }

        //protected abstract void ValidateSelf(); TODO

        public void Validate()
        {
            this.ValidationErrors.Clear();
            //this.ValidateSelf();
            this.isValid = this.ValidationErrors.IsValid;
            this.OnPropertyChanged("IsValid");
            this.OnPropertyChanged("ValidationErrors");
        }
    }
}
