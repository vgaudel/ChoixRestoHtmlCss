﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixResto.ViewModels
{
    public class RestaurantCheckBoxViewModel
    {
        public int Id { get; set; }
        public string NomEtTelephone { get; set; }
        public bool EstSelectionne { get; set; }
    }
}