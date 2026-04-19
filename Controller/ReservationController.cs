namespace Task5.Controller;

using Microsoft.AspNetCore.Mvc;
using Task5.Models;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = DataStore.Reservations.AsEnumerable();

        if (date.HasValue)
        {
            reservations = reservations.Where(r => r.Date == date.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            reservations = reservations.Where(r =>
                r.Status != null &&
                r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        if (roomId.HasValue)
        {
            reservations = reservations.Where(r => r.RoomId == roomId.Value);
        }

        return Ok(reservations.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Reservation> GetById(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation == null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Create([FromBody] Reservation reservation)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

        if (room == null)
        {
            return BadRequest("Room does not exist.");
        }

        if (!room.IsActive)
        {
            return BadRequest("Room is inactive.");
        }

        var overlaps = DataStore.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            reservation.StartTime < r.EndTime &&
            reservation.EndTime > r.StartTime);

        if (overlaps)
        {
            return Conflict("Reservation overlaps with an existing reservation.");
        }

        reservation.Id = DataStore.NextReservationId;
        DataStore.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public ActionResult<Reservation> Update(int id, [FromBody] Reservation updatedReservation)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation == null)
        {
            return NotFound();
        }

        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);

        if (room == null)
        {
            return BadRequest("Room does not exist.");
        }

        if (!room.IsActive)
        {
            return BadRequest("Room is inactive.");
        }

        var overlaps = DataStore.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == updatedReservation.RoomId &&
            r.Date == updatedReservation.Date &&
            updatedReservation.StartTime < r.EndTime &&
            updatedReservation.EndTime > r.StartTime);

        if (overlaps)
        {
            return Conflict("Reservation overlaps with an existing reservation.");
        }

        reservation.RoomId = updatedReservation.RoomId;
        reservation.OrganizerName = updatedReservation.OrganizerName;
        reservation.Topic = updatedReservation.Topic;
        reservation.Date = updatedReservation.Date;
        reservation.StartTime = updatedReservation.StartTime;
        reservation.EndTime = updatedReservation.EndTime;
        reservation.Status = updatedReservation.Status;

        return Ok(reservation);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation == null)
        {
            return NotFound();
        }

        DataStore.Reservations.Remove(reservation);

        return NoContent();
    }
}