﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BankingSystem.DB.Entities;

namespace BankingSystem.Features.InternetBank.User.GetUserInfo
{
    public class GetCardsResponse
    {
        public CardStatus CardStatus { get; set; }
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CVV { get; set; }
        public string Pin { get; set; }
    }

    public enum CardStatus
    {
       Valid, ExpiresSoon, Expired
    }
}
