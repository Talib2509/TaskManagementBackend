using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMnagementBackend.Domain.Enums
{
    public enum TaskVisibility
    {
        Private = 1, // Видно только создателю (Team Lead) и назначенным исполнителям
        Public = 2 // Видно всей команде
    }
}
