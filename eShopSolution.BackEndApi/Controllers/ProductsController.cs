﻿using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndApi.Controllers
{
	// api/products
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IPublicProductService _publicProductService;
		private readonly IManageProductService _manageProductService;

		public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
		{
			_publicProductService = publicProductService;
			_manageProductService = manageProductService;
		}

		// http://localhost:port/products?pageIndex=1&pageSize=10&CategoryId=
		[HttpGet("{languageId}")]
		public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
		{
			var products = await _publicProductService.GetAllByCategoryId(languageId, request);
			return Ok(products);
		}

		//http://localhost:port/product/1
		[HttpGet("{productId}/{languageId}")]
		public async Task<IActionResult> GetById(int productId, string languageId)
		{
			var product = await _manageProductService.GetById(productId, languageId);
			if (product == null)
				return BadRequest("Cannot find product");
			return Ok(product);
		}

		//http://localhost:port/product/1
		[HttpPost]
		public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var productId = await _manageProductService.Create(request);
			if (productId == 0)
				return BadRequest();
			var product = await _manageProductService.GetById(productId, request.LanguageId);
			return CreatedAtAction(nameof(GetById), new { id = productId }, product);
		}

		//http://localhost:port/product
		[HttpPut]
		public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var affectedResult = await _manageProductService.Update(request);
			if (affectedResult == 0)
				return BadRequest();
			return Ok();
		}

		//http://localhost:port/product/1
		[HttpDelete("{productId}")]
		public async Task<IActionResult> Delete(int productId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var affectedResult = await _manageProductService.Delete(productId);
			if (affectedResult == 0)
				return BadRequest();
			return Ok();
		}

		//http://localhost:port/product/1
		[HttpPatch("{id}/{newPrice}")]
		public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var affectedResult = await _manageProductService.UpdatePrice(productId		, newPrice);
			if (affectedResult)
				return Ok();
			return BadRequest();

		}

		//Images
		[HttpPost("{productId}/images")]
		public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var imageId = await _manageProductService.AddImage(productId, request);
			if (imageId == 0)
				return BadRequest();

			var image = await _manageProductService.GetImageById(imageId);

			return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
		}

		[HttpPut("{productId}/images/{imageId}")]
		public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var result = await _manageProductService.UpdateImage(imageId, request);
			if (result == 0)
				return BadRequest();
			return Ok();
		}
		//Images
		[HttpDelete("{productId}/images/{imageId}")]
		public async Task<IActionResult> RemoveImage(int imageId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var result = await _manageProductService.RemoveImage(imageId);
			if (result == 0)
				return BadRequest();
			return Ok();
		}

		//Images
		[HttpGet("{productId}/images/{imageId}")]
		public async Task<IActionResult> GetImageById(int productId, int imageId)
		{
			var result = await _manageProductService.GetImageById(imageId);
			if (result == null)
				return BadRequest("Cannot find Image");
			return Ok();
		}
	}
}