using DataCa;
using EntityCa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopCa
{
    public class CN_Customer
    {
        private CD_Customer objDataCa = new CD_Customer();
       

        public int Register(Customer obj, out string Message)
        {
            Message = string.Empty;
            if (string.IsNullOrEmpty(obj.Name) || string.IsNullOrWhiteSpace(obj.Name))
            {
                Message = "Name is required";
            }
            else if (string.IsNullOrEmpty(obj.LastName) || string.IsNullOrWhiteSpace(obj.LastName))
            {
                Message = "Last Name is required";
            }
            else if (string.IsNullOrEmpty(obj.Email) || string.IsNullOrWhiteSpace(obj.Email))
            {
                Message = "Email is required";
            }

            if (string.IsNullOrEmpty(Message))
            {
                obj.Password = CN_Resources.ConvertSha256(obj.Password);
                return objDataCa.Register(obj, out Message);

            }
            else
            {
                return 0;
            }

        }
        public List<Customer> Listar()
        {

            return objDataCa.Listar();
        }

        public bool ChangePassword(int idcustomer, string newpassword, out string Message)
        {
            return objDataCa.ChangePassword(idcustomer, newpassword, out Message);
        }

        public bool ResetPassword(int idcustomer, string email, out string Message)
        {
            Message = string.Empty;
            string newpassword = CN_Resources.CreatePassword();
            bool result = objDataCa.ResetPassword(idcustomer, CN_Resources.ConvertSha256(newpassword), out Message);

            if (result)
            {
                string subject = "Password reset";
                string message_email = "<h3>Your account has been successfully reset</h3></br><p>Your password to access is: !password!</p>";
                message_email = message_email.Replace("!password!", newpassword);
                bool res = CN_Resources.SendMail(email, subject, message_email);

                if (res)
                {
                    return true;
                }
                else
                {
                    Message = "The email could not be sent.";
                    return false;
                }

            }
            else
            {
                Message = "The password could not be reset.";
                return false;
            }



        }

    }
}
