using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	/// <summary>
	/// делегат для создания события загрузки формы клиента
	/// </summary>
	public delegate void GamerIsLoaded(INetworkClient client);

	/// <summary>
	/// делегат для создания события о том, что игровая логика комнаты завершила свою работу
	/// </summary>
	public delegate void RoomLogicEndWork(IRoomLogic roomLogic);

	/// <summary>
	/// Делегат для создания события о том, что игровая комната завершила свою работу
	/// </summary>
	/// <param name="room"></param>
	public delegate void RoomEndWork(IRoom room);

	/// <summary>
	/// делегат для создания события, уведомляющего, что игрок завершил игру
	/// </summary>
	/// <param name="player"></param>
	public delegate void PlayerDeleted(IPlayer player);

	/// <summary>
	/// делегат для создания событий, уведомляющего, что участие игрока в игре завершено
	/// </summary>
	/// <param name=""></param>
	public delegate void NetworkClientEndWork(INetworkClient networkClient);

	public delegate void GameObjectDeleted(IGameObject gameObject);

	public delegate void NetorkClientDisconnect(INetworkClient client);

}
