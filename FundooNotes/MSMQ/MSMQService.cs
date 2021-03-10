using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLayer.EmailMessageModel;
using CommonLayer.Model;
using Experimental.System.Messaging;
using FundooNotes.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FundooNotes.MSMQ
{
    public class MSMQService
    {
        EmailService emailService;
        MessageQueue queue = new MessageQueue(@".\private$\FunDooNotes");

        public MSMQService(IConfiguration config)
        {
            emailService = new EmailService(config);
        }

        public void SendPasswordResetMessage(ResetLinkEmailModel resetLink)
        {
            try
            {
                if (!MessageQueue.Exists(queue.Path))
                {
                    MessageQueue.Create(queue.Path);
                }
                queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                Message msg = new Message
                {
                    Label = "password reset link",
                    Body = JsonConvert.SerializeObject(resetLink)
                };
                queue.Send(msg);
                queue.ReceiveCompleted += Queue_ReceiveCompleted;
                queue.BeginReceive(TimeSpan.FromSeconds(10.0));
                queue.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        void Queue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {          
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                ResetLinkEmailModel model = JsonConvert.DeserializeObject<ResetLinkEmailModel>(msg.Body.ToString());
                emailService.SendPasswordResetLinkEmail(model);
                queue.BeginReceive(TimeSpan.FromSeconds(10.0));
            }
            catch (Exception)
            {

            }
        }
    }
}
