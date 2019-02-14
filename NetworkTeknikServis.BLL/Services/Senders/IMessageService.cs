using Microsoft.AspNet.Identity;
using NetworkTeknikServis.MODELS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkTeknikServis.BLL.Services.Senders
{
    public interface IMessageService
    {
        MessageStates MessageState { get; }

        Task SendAsync(IdentityMessage message, params string[] contacts);
        void Send(IdentityMessage message, params string[] contacts);
    }
}
