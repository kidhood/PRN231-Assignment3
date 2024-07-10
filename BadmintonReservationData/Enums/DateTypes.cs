﻿using System.ComponentModel;

namespace BadmintonReservationData.Enums
{
    public enum DateTypes
    {
        [Description("Monday")]
        Monday = 1,

        [Description("Tuesday")]
        Tuesday = 2,

        [Description("Wednesday")]
        Wednesday = 3,

        [Description("Thursday")]
        Thursday = 4,

        [Description("Friday")]
        Friday = 5,

        [Description("Saturday")]
        Saturday = 6,

        [Description("Sunday")]
        Sunday = 7,
    }
}
