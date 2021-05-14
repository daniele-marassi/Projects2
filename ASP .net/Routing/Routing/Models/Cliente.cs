using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Routing.Models
{
    public class Cliente
    {
        string cognome = "";
        int eta = 0;

        public Cliente(string _cognome, int _eta)
        { cognome = _cognome; eta = _eta; }

        public string getCognome() { return cognome; }
    }
}