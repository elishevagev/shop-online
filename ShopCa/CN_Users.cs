using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCa;
using EntityCa;

namespace ShopCa
{
    public class CN_Users
    {
        private CD_Users objDataCa = new CD_Users();
        public List<User_Information> Listar()
        {

            return objDataCa.Listar();
        }

        public int Register(User_Information obj, out string Message)
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

                string password = CN_Resources.CreatePassword();
                string subject = "Account creation";
                string message_email = "<h3>Your account was created successfully</h3></br><p>Your password to access is: !password!</p>";
                message_email = message_email.Replace("!password!", password);
                bool res = CN_Resources.SendMail(obj.Email, subject, message_email);
                if (res)
                {
                    obj.Password = CN_Resources.ConvertSha256(password);
                    return objDataCa.Register(obj, out Message);
                }else
                {
                    Message = "The email could not be sent.";
                    return 0;
                }
                
            }
            else
            {
                return 0;
            }

        }
        public bool Edit(User_Information obj, out string Message)
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
                return objDataCa.Edit(obj, out Message);
            }
            else
            {
                return false;
            }
        }

        public bool Delete( int id, out string Message)
        {
            return objDataCa.Delete(id, out Message);
        }


        public bool ChangePassword(int iduser, string newpassword, out string Message)
        {
            return objDataCa.ChangePassword(iduser,newpassword, out Message);
        }

        public bool ResetPassword(int iduser, string email, out string Message)
        {
            Message = string.Empty;
            string newpassword = CN_Resources.CreatePassword();
            bool result = objDataCa.ResetPassword(iduser, CN_Resources.ConvertSha256(newpassword), out Message);

            if(result)
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

            }else
            {
                Message = "The password could not be reset.";
                return false;
            }



        }
    }
}
