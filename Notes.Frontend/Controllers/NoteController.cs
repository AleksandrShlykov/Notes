﻿using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Frontend.ViewModel;
using Notes.WebAPI.Modelas;
using System.Net.Http.Headers;

namespace Notes.Frontend.Controllers
{
    public class NoteController : Controller
    {
        private readonly ILogger<NoteController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        string _baseUrl = "https://localhost:7157/api/";
        string _apiVersion = "1.0/";
        private NoteListVm _notes;

        public NoteController(ILogger<NoteController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LoginPage()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/note/getallnote");
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public  IActionResult GoToLogin()
        {
            return  Redirect("/note/getallnote");
        }
        [Authorize]
        public IActionResult Logout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllNote()
        {

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_baseUrl + _apiVersion);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var model = new ClaimManager(HttpContext, User);
            client.SetBearerToken(model.AccessToken);
            var response = await client.GetAsync("https://localhost:7157/api/1.0/note");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                _notes = JsonConvert.DeserializeObject<NoteListVm>(content);

            }
            return View("AllNote", _notes);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetNoteDetails(Guid id)
        {
            var note = new NoteDetailsVm();
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var model = new ClaimManager(HttpContext, User);
            client.SetBearerToken(model.AccessToken);
            var response = await client.GetAsync($"https://localhost:7157/api/1.0/note/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                note = JsonConvert.DeserializeObject<NoteDetailsVm>(content);
            }

            return View("NoteDetails", note);
        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateNote()
        {

            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateNoteDto createNoteDto)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_baseUrl + _apiVersion);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var model = new ClaimManager(HttpContext, User);
            client.SetBearerToken(model.AccessToken);
            var response = await client.PostAsJsonAsync("note", createNoteDto);
            return Redirect("/note/getallnote");

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateNote(Guid id)
        {
            var note = new UpdateNoteDto();
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var model = new ClaimManager(HttpContext, User);
            client.SetBearerToken(model.AccessToken);
            var response = await client.GetAsync($"https://localhost:7157/api/1.0/note/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                note = JsonConvert.DeserializeObject<UpdateNoteDto>(content);
            }
            return View("UpdateNote", note);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateNote(UpdateNoteDto updateNoteDto)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_baseUrl + _apiVersion);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var model = new ClaimManager(HttpContext, User);
            client.SetBearerToken(model.AccessToken);
            var response = await client.PutAsJsonAsync("note", updateNoteDto);
            
            return View();
        }
        [Authorize]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_baseUrl + _apiVersion);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var model = new ClaimManager(HttpContext, User);
            client.SetBearerToken(model.AccessToken);
            var response = await client.DeleteAsync($"note/{id}");
            return Redirect("/note/getallnote");
        }
    }
}