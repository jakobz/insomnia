using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insomnia;
using Insomnia.Tests.Example.Infrastructure;
using Insomnia.Tests.Example.Models;
using Insomnia.Tests.Example.ViewModels;

namespace Insomnia.Tests.Example.Controllers
{
    class UserSettingsController : BaseFormController<UserProfile, UserProfileSettingsVM>
    {
        protected override UserProfile NewModel()
        {
            return new UserProfile();
        }

        protected override UserProfile LoadModel(int id)
        {
            return Db.UserProfiles.Find(id);
        }

        protected override void Map(ClassMapper<UserProfile, UserProfileSettingsVM> map)
        {
            map.Scalar(m => m.TimeZoneId, vm => vm.TimeZoneID);
        }
    }
}
