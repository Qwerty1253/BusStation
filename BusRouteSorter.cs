// BusRouteSorter.cs
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BusStation
{
    public class BusRouteSorter
    {
        // Сортировка по номеру автобуса
        public void SortByBusNumber(ObservableCollection<BusRoute> routes, bool ascending = true)
        {
            var sorted = ascending ?
                routes.OrderBy(r => r.BusNumber).ToList() :
                routes.OrderByDescending(r => r.BusNumber).ToList();

            UpdateCollection(routes, sorted);
        }

        // Сортировка по пункту отправления
        public void SortByDeparturePoint(ObservableCollection<BusRoute> routes, bool ascending = true)
        {
            var sorted = ascending ?
                routes.OrderBy(r => r.DeparturePoint).ToList() :
                routes.OrderByDescending(r => r.DeparturePoint).ToList();

            UpdateCollection(routes, sorted);
        }

        // Сортировка по пункту назначения
        public void SortByDestinationPoint(ObservableCollection<BusRoute> routes, bool ascending = true)
        {
            var sorted = ascending ?
                routes.OrderBy(r => r.DestinationPoint).ToList() :
                routes.OrderByDescending(r => r.DestinationPoint).ToList();

            UpdateCollection(routes, sorted);
        }

        // Сортировка по времени отправления
        public void SortByDepartureTime(ObservableCollection<BusRoute> routes, bool ascending = true)
        {
            var sorted = ascending ?
                routes.OrderBy(r => r.DepartureTime).ToList() :
                routes.OrderByDescending(r => r.DepartureTime).ToList();

            UpdateCollection(routes, sorted);
        }

        // Сортировка по времени прибытия
        public void SortByArrivalTime(ObservableCollection<BusRoute> routes, bool ascending = true)
        {
            var sorted = ascending ?
                routes.OrderBy(r => r.ArrivalTime).ToList() :
                routes.OrderByDescending(r => r.ArrivalTime).ToList();

            UpdateCollection(routes, sorted);
        }

        // Сортировка по цене билета
        public void SortByTicketPrice(ObservableCollection<BusRoute> routes, bool ascending = true)
        {
            var sorted = ascending ?
                routes.OrderBy(r => r.TicketPrice).ToList() :
                routes.OrderByDescending(r => r.TicketPrice).ToList();

            UpdateCollection(routes, sorted);
        }

        // Обновление ObservableCollection после сортировки
        private void UpdateCollection(ObservableCollection<BusRoute> collection, List<BusRoute> sorted)
        {
            collection.Clear();
            foreach (var item in sorted)
            {
                collection.Add(item);
            }
        }
    }
}