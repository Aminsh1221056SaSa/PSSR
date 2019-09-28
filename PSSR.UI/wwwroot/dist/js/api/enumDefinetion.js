var enumDefinetion = enumDefinetion || (function () {
    return {
        getProjectType: function (eId) {
            return getProjectType(eId);
        }
    };

    function getProjectType(eId)
    {
        switch (eId)
        {
            case 1001:
                return "Oil Platform";
            case 1002:
                return "Gas Platform";
            case 1003:
                return "Refinery";
            case 1004:
                return "Other";
        }
    }
}());