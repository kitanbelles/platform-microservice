using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper; 
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using PlatformService.AsyncDataServices;
using PlatformService.DTO;

namespace PlatformService.Controllers; 

[Route("api/platforms")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatformRepo repository, IMapper mapper,
                                ICommandDataClient commandDataClient, 
                                IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("-->Getting Platforms ....");
        var platformItems = _repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems   ));
    }

    [HttpGet("{id:int}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        Console.WriteLine("-->Getting one Platform ....");
        var platformItem = _repository.GetPlatformById(id);
        if(platformItem != null)
        {
            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatformById(PlatformCreateDto platformCreateDto)
    {
        Console.WriteLine("-->Creating one Platform ....");

        var platformModel = _mapper.Map<Platform>(platformCreateDto); 
        _repository.CreatePlatform(platformModel);
        _repository.SaveChanges();

        var PlatformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
        // Send Sync Measage 
        try
        {
            await _commandDataClient.SendPlatformToCommand(PlatformReadDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"-----> Could not send synchronously: {ex.Message}");
        }
        // Send Async Measage 
        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(PlatformReadDto);
            platformPublishedDto.Event = "PlatformPublished";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"-----> Could not send asynchronously: {ex.Message}");
        }
        return CreatedAtRoute(nameof(GetPlatformById), new {id = PlatformReadDto.Id}, PlatformReadDto);
    }
}