using CommandsService.Models;

namespace CommandsService.Data; 

public interface ICommandRepo
{
    bool SaveChanges();

    // Platforms stuff 
    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform plat);
    bool PlatformExists(int platformId);
    bool ExternalPlatformExists(int externalPlatformId);

    // Commands stuff 
    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command GetCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);
}