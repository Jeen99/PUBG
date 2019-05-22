using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Client;
using CommonLibrary.AutorizationMessages;
using CommonLibrary;

namespace BattleRoyalClient
{
	
	public class AuthorizationModel : IAuthorizationModerForView, IAuthorizationModelForController
	{
		private string _nickName = "";
		private string _password = "";
		private bool _saveAutorizationData = false;
		public event ChangeAutorizationModel AutorizationModelChange;

		public string NickName
		{
			get { return _nickName; }
			set
			{
				_nickName = value;
				CreateChangeModel(TypesChangeAutorizationModel.NickName);
			}
		} 

		public string Password {
			get { return _password; }
			set
			{
				_password = value;
				CreateChangeModel(TypesChangeAutorizationModel.Password);
			}
		} 

		public bool SaveAutorizationData
		{
			get { return _saveAutorizationData; }
		}

		public StatesAutorizationModel State { get; private set; } = StatesAutorizationModel.NoAutorization;

		public void CreateChangeModel(TypesChangeAutorizationModel type)
		{
			AutorizationModelChange?.Invoke(type);
		}

		public AuthorizationModel()
		{
			
		}

		public void ChangeSaveAutorizationData()
		{
			if (SaveAutorizationData) _saveAutorizationData = false;
			else _saveAutorizationData = true;
			CreateChangeModel( TypesChangeAutorizationModel.SaveAutorizationData);
		}

		public void Load()
		{
			string login;
			string pass;

			if (AuthorizationData.LoadAuthorizationData(out login, out pass))
			{
				_nickName = login;
				_password = pass;
				_saveAutorizationData = true;			
			}

			CreateChangeModel(TypesChangeAutorizationModel.All);
		}

		public void Update(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesMessage.ResultAutorization:
					Handler_ResultAutorization(msg);
					break;
			}
		}

		private void Handler_ResultAutorization(IMessage msg)
		{
			if (msg.Result)
			{
				State = StatesAutorizationModel.SuccessAutorization;
			}
			else
			{
				State = StatesAutorizationModel.IncorrectData;
			}
			CreateChangeModel( TypesChangeAutorizationModel.State);

		}

		public void SaveAndClearModel()
		{
			if (_saveAutorizationData)
			{
				AuthorizationData.SaveAuthorizationData(_nickName, _password);
			}
			else
			{
				AuthorizationData.DeleteAuthorizationData();
			}

			_saveAutorizationData = false;
			State = StatesAutorizationModel.NoAutorization;
			AutorizationModelChange = null;
			_nickName = "";
			_password = "";
		}

		public void HappenedLossConnectToServer()
		{
			State =  StatesAutorizationModel.ErrorConnect;
			CreateChangeModel( TypesChangeAutorizationModel.State);
		}
	}
	public delegate void ChangeAutorizationModel(TypesChangeAutorizationModel type);

	public enum TypesChangeAutorizationModel
	{
		NickName,
		Password,
		SaveAutorizationData,
		State,
		All
	}

	public enum StatesAutorizationModel
	{
		NoAutorization,
		ErrorConnect,
		IncorrectData,
		SuccessAutorization
	}


}
