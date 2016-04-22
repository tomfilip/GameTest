using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class ItemOutsideTableException : Exception
    {
        public ItemOutsideTableException(TableItem<GameTable> item)
        {
            this.TableItem = item;
        }

        public TableItem<GameTable> TableItem { get; private set; }
    }
}
