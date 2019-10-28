var enumDefinetion = enumDefinetion || (function () {
    return {
        getProjectType: function (eId) {
            return getProjectType(eId);
        },
        getFormDocumentType: function (ftId) {
            return getFormDocumentType(ftId);
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

    function getFormDocumentType(ftId) {
        switch (ftId) {
            case 1:
                return "Test";
            case 2:
                return "Check";
        }
    }
}());