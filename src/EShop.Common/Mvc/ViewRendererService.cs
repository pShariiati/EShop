﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EShop.Common.Mvc
{
    public class ViewRendererService : IViewRendererService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ViewRendererService(
                    IRazorViewEngine viewEngine,
                    ITempDataProvider tempDataProvider,
                    IServiceProvider serviceProvider,
                    IHttpContextAccessor httpContextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> RenderViewToStringAsync(string viewNameOrPath)
        {
            return RenderViewToStringAsync(viewNameOrPath, string.Empty);
        }

        public async Task<string> RenderViewToStringAsync<TModel>(string viewNameOrPath, TModel model)
        {
            var actionContext = GetActionContext();

            var viewEngineResult = _viewEngine.FindView(actionContext, viewNameOrPath, isMainPage: false);
            if (!viewEngineResult.Success)
            {
                viewEngineResult = _viewEngine.GetView("~/", viewNameOrPath, isMainPage: false);
                if (!viewEngineResult.Success)
                {
                    throw new FileNotFoundException($"Couldn't find '{viewNameOrPath}'");
                }
            }

            var view = viewEngineResult.View;
            using var output = new StringWriter();
            var viewDataDictionary = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
                actionContext,
                view,
                viewDataDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                output,
                new HtmlHelperOptions());
            await view.RenderAsync(viewContext).ConfigureAwait(false);
            return output.ToString();
        }

        private ActionContext GetActionContext()
        {
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext != null)
            {
                return new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor());
            }

            httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}