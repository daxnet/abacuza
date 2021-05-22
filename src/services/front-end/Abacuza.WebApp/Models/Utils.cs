using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.WebApp.Models
{
    public static class Utils
    {
        public enum ToastMessageType
        {
            Info,
            Warning,
            Error,
            Success
        }

        public static void ShowToastMessage(this Controller controller, 
            string message, 
            string title, 
            string? subtitle = null, 
            ToastMessageType type = ToastMessageType.Info)
        {
            controller.ViewBag.ToastMessage = message;
            controller.ViewBag.ToastTitle = title;
            controller.ViewBag.ToastSubtitle = subtitle ?? string.Empty;
            controller.ViewBag.ToastType = type switch
            {
                ToastMessageType.Info => "bg-info",
                ToastMessageType.Warning => "bg-warning",
                ToastMessageType.Success => "bg-success",
                ToastMessageType.Error => "bg-danger",
                _ => "bg-info"
            };

        }
    }
}
