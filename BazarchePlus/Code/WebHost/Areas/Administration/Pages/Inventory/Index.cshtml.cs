using IM._Application.Contracts.Inventory;
using IM._Application.Contracts.Inventory.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SM._Application.Contracts.Product;

namespace WebHost.Areas.Administration.Pages.Inventory
{
    //[Authorize(Roles = Roles.Administrator)]
    public class IndexModel : PageModel
    {
        [TempData] public string Message { get; set; }
        public InventorySearchModel SearchModel;
        public List<InventoryViewModel> Inventory;
        public SelectList Products;

        private readonly IProductApplication _productApplication;
        private readonly IInventoryApplication _inventoryApplication;

        public IndexModel(IProductApplication productApplication, IInventoryApplication inventoryApplication)
        {
            _productApplication = productApplication;
            _inventoryApplication = inventoryApplication;
        }

        //[NeedsPermission(InventoryPermissions.ListInventory)]
        public async Task OnGet(InventorySearchModel searchModel,CancellationToken cancellationToken)
        {
            Products = new SelectList(await _productApplication.GetProducts(cancellationToken), "Id", "Name");
            Inventory = await _inventoryApplication.Search(searchModel,cancellationToken);
        }

        //[NeedsPermission(InventoryPermissions.CreateInventory)]
        public async Task<IActionResult> OnGetCreate(CancellationToken cancellationToken)
        {
            var command = new CreateInventory
            {
                Products = await _productApplication.GetProducts(cancellationToken)
            };
            return Partial("./Create", command);
        }

        //[NeedsPermission(InventoryPermissions.CreateInventory)]
        public async Task<JsonResult> OnPostCreate(CreateInventory command,CancellationToken cancellationToken)
        {
            var result = await _inventoryApplication.Create(command,cancellationToken);
            return new JsonResult(result);
        }

        public async Task<IActionResult> OnGetEdit(long id,CancellationToken cancellationToken)
        {
            var inventory = await _inventoryApplication.GetDetails(id,cancellationToken);
            inventory.Products = await _productApplication.GetProducts(cancellationToken);
            return Partial("Edit", inventory);
        }

        //[NeedsPermission(InventoryPermissions.EditInventory)]
        public async Task<JsonResult> OnPostEdit(EditInventory command,CancellationToken cancellationToken)
        {
            var result =await _inventoryApplication.Edit(command,cancellationToken);
            return new JsonResult(result);
        }

        public IActionResult OnGetIncrease(long id)
        {
            var command = new IncreaseInventory()
            {
                InventoryId = id
            };
            return Partial("Increase", command);
        }

        //[NeedsPermission(InventoryPermissions.Increase)]
        public async Task<JsonResult> OnPostIncrease(IncreaseInventory command,CancellationToken cancellationToken)
        {
            var result = await _inventoryApplication.Increase(command ,cancellationToken);
            return new JsonResult(result);
        }

        public IActionResult OnGetReduce(long id)
        {
            var command = new ReduceInventory()
            {
                InventoryId = id
            };
            return Partial("Reduce", command);
        }

        //[NeedsPermission(InventoryPermissions.Reduce)]
        public async Task<JsonResult> OnPostReduce(ReduceInventory command,CancellationToken cancellationToken)
        {
            var result = await _inventoryApplication.Reduce(command,cancellationToken);
            return new JsonResult(result);
        }

        //[NeedsPermission(InventoryPermissions.OperationLog)]
        public async Task<IActionResult> OnGetLog(long id,CancellationToken cancellationToken)
        {
            var log = await _inventoryApplication.GetOperationLog(id,cancellationToken);
            return Partial("OperationLog", log);
        }
    }
}