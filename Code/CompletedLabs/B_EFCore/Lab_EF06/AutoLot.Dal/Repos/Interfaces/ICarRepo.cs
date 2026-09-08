// Copyright Information
// ==================================
// AutoLot - AutoLot.Dal - ICarRepo.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/05
// ==================================

namespace AutoLot.Dal.Repos.Interfaces;

public interface ICarRepo : IBaseRepo<Car>
{
    List<Car> GetAllByAsList(
        int makeId);

    // Returns deferred IQueryable — DbContext must remain alive until materialized.
    IQueryable<Car> GetAllByAsQueryable(
        int makeId);

    string GetPetName(
        int id);

    int SetAllDrivableCarsColorAndMakeId(
        string color,
        int makeId);

    int DeleteNonDrivableCars();
}