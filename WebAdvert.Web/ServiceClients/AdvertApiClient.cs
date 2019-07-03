using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebAdvert.AdvertApi.Dto;
using WebAdvert.Web.ServiceClients.Models;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
        {
            _configuration = configuration;
            _httpClient = client;
            _mapper = mapper;

            _httpClient.BaseAddress = new Uri(_configuration.GetValue<string>("AdvertUrls:BaseAddress"));
        }

        public async Task<CreateAdvertResponseModel> CreateAsync(AdvertModel model)
        {
            var url = _configuration.GetValue<string>("AdvertUrls:Advert:Create");

            var advertDto = _mapper.Map<AdvertDto>(model);

            var response = await _httpClient.PostAsJsonAsync(url, advertDto);

            var createResponseDto = await response.Content.ReadAsAsync<CreateAdvertResponseDto>();

            return _mapper.Map<CreateAdvertResponseModel>(createResponseDto);
        }

        public async Task<bool> ConfirmAsync(ConfirmAdvertModel model)
        {
            var url = _configuration.GetValue<string>("AdvertUrls:Advert:Confirm");

            var confirmAdvertDto = _mapper.Map<ConfirmAdvertDto>(model);

            var response = await _httpClient.PutAsJsonAsync(url, confirmAdvertDto);

            return response.IsSuccessStatusCode;
        }
    }
}
