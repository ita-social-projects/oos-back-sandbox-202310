﻿global using System;
global using System.Globalization;
global using System.Linq;
global using System.Net;
global using System.Reflection;
global using System.Threading.Tasks;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http.Extensions;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Localization;
global using Microsoft.AspNetCore.Mvc.Razor;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using MySqlConnector;
global using OutOfSchool.AuthCommon.Services.Interfaces;
global using OutOfSchool.Common;
global using OutOfSchool.Common.Config;
global using OutOfSchool.Common.Extensions;
global using OutOfSchool.Common.Extensions.Startup;
global using OutOfSchool.Common.PermissionsModule;
global using OutOfSchool.EmailSender;
global using OutOfSchool.RazorTemplatesData.Services;
global using OutOfSchool.Services;
global using OutOfSchool.Services.Extensions;
global using OutOfSchool.Services.Models;
global using OutOfSchool.Services.Repository;
