using PlatformService.DTO;

namespace PlatformService.AsyncDataServices; 

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}