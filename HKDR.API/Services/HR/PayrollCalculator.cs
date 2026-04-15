namespace HKDR.API.Services.HR
{
    public static class PayrollCalculator
    {
        public static void Calculate(PayrollTransaction tx)
        {
            // ضمان اجتماعي (مثال)
            tx.TaxAmount = tx.GrossSalary * 0.05m;

            // خصومات أخرى
            tx.OtherDeductions = tx.LoanAmount;

            // الصافي
            tx.NetSalary =
                tx.GrossSalary
                - tx.TaxAmount
                - tx.OtherDeductions;
        }
    }

}
