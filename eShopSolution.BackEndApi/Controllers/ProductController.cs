﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackEndApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IPublicProductService _publicProductService;

		public ProductController(IPublicProductService publicProductService)
		{
			_publicProductService = publicProductService;
		}

		[HttpGet]
		public async Task <IActionResult> Get()
		{
			var productList = await _publicProductService.GetAll();
			return Ok(productList);
		}
	}
}