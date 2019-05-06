using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.AutorizationMessages
{
	[Serializable]
	public class ResultAuthorization : Message
	{
		public override bool Result { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.ResultAutorization;

		public ResultAuthorization(bool resultAutorization)
		{
			Result = resultAutorization;
		}
	}
}
