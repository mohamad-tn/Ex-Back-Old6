using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Bwr.Exchange.Roles.Dto;
using Bwr.Exchange.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;

namespace Bwr.Exchange.Roles
{
    public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>
    {
        Task<ReadGrudDto> GetForGrid([FromBody] DataManagerRequest dm);
        Task<ListResultDto<PermissionDto>> GetAllPermissions();
        IList<FlatPermissionDto> GetPermissions();
        Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input);
        Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input);
    }
}
