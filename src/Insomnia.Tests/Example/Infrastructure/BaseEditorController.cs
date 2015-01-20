using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insomnia;
using Insomnia.Mappers;
using Insomnia.Tests.Example.Context;
using Insomnia.Tests.Example.Models;

namespace Insomnia.Tests.Example.Infrastructure
{
    abstract class BaseFormController<M, VM> : IDisposable
        where M : class where VM : class, new()
    {
        // Ancestors' interface
        protected abstract M NewModel();
        protected abstract M LoadModel(int id);
        protected abstract void Map(ClassMapper<M, VM> map);

        // DB context management
        private TestDbContext context;
        protected ITestDbContext Db { get { return context; } }

        public BaseFormController() 
        {
            context = new TestDbContext();
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing && context != null)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        ~BaseFormController()
        {
            Dispose(false);
        }

        // Common API methods for forms

        public FormResponse<VM> New()
        {
            var model = NewModel();
            return MapModelToResponse(model);
        }

        public FormResponse<VM> Get(FormRequest<VM> request)
        {
            var model = LoadModel(request.ID.Value);
            return MapModelToResponse(model);
        }

        public FormResponse<VM> Save(FormRequest<VM> request)
        {
            var model = LoadModel(request.ID.Value);

            var mapper = new UpdateMapper<M, VM>(model, request.ViewState);
            Map(mapper);                     

            var newViewModel = MapModelToResponse(model);
            context.SaveChanges();
            return newViewModel;
        }

        FormResponse<VM> MapModelToResponse(M model)
        {
            var viewModel = new VM();
            var mapper = new GetMapper<M, VM>(model, viewModel);
            Map(mapper);
            return new FormResponse<VM>() {
                ViewState = viewModel
            };
        }
    }
}
