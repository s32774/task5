namespace Task5.Controller;

using Microsoft.AspNetCore.Mvc;
using Task5.Models;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var rooms = DataStore.Rooms.AsEnumerable();

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
        }

        if (activeOnly == true)
        {
            rooms = rooms.Where(r => r.IsActive);
        }

        return Ok(rooms.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);

        if (room == null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetByBuilding(string buildingCode)
    {
        var rooms = DataStore.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public ActionResult<Room> Create([FromBody] Room room)
    {
        room.Id = DataStore.NextRoomId;
        DataStore.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public ActionResult<Room> Update(int id, [FromBody] Room updatedRoom)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);

        if (room == null)
        {
            return NotFound();
        }

        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor = updatedRoom.Floor;
        room.Capacity = updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);

        if (room == null)
        {
            return NotFound();
        }

        var hasReservations = DataStore.Reservations.Any(r => r.RoomId == id);

        if (hasReservations)
        {
            return Conflict("Cannot delete room because it has reservations.");
        }

        DataStore.Rooms.Remove(room);

        return NoContent();
    }
}