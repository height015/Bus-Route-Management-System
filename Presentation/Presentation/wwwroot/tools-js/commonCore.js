
class JK_ServiceCore {
    #coreItem = {
        servicePath: "",
        detailHistPath: "",
        saveConfirmAlert: "",
        saveSuccessAlert: "",
        updateSuccessAlert: "",
        deleteConfirmAlert: "",
        deleteSuccessAlert: "",
    };

    constructor(coreItemObj) {
        this.#coreItem = coreItemObj;
    }

    isValid() {
        if (
            this.#coreItem === null ||
            this.#coreItem.servicePath === "" ||
            this.#coreItem.servicePath.length < 3
        )
            return false;
        // Check other conditions and set default values if needed
        return true;
    }


 displayDashView() {
        blockMainView();

        fetch(this.#coreItem.servicePath + "list-view")
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.text();
            })
            .then((data) => {
                document.getElementById("dvTableListView").innerHTML = data;
                unblockMainView();
            })
            .catch((error) => {
                console.error("Error fetching data:", error);
                unblockMainView();
            });
    }

 displayDashList() {
        blockMainView();

        fetch(this.#coreItem.servicePath + "list")
            .then((response) => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.text();
            })
            .then((data) => {
                document.getElementById("dvListView").innerHTML = data;
                unblockMainView();
            })
            .catch((error) => {
                console.error("Error fetching data:", error);
                unblockMainView();
            });
    }

 displayDetailView(id) {
        if (id === null || id === "" || parseInt(id) < 1) return false;
        blockMainView();

        const itemResponse = fetch(this.#coreItem.servicePath + "item-view?itemId=" + parseInt(id));
        const itemListResponse = fetch(this.#coreItem.servicePath + "external-list?itemId=" + parseInt(id));

        Promise.all([itemResponse, itemListResponse])
            .then((responses) => {
                return Promise.all(responses.map((response) => response.text()));
            })
            .then(([item, itemList]) => {
                document.getElementById("dvItemView").innerHTML = item;
                document.getElementById("dvItemList").innerHTML = itemList;
                unblockMainView();
            })
            .catch((error) => {
                console.error("Error fetching data:", error);
                unblockMainView();
            });
    }
    
 displayDetailHistView(id, type) {
        if (id === null || id === "" || parseInt(id) < 1) return false;
        if (type === null || type === "" || parseInt(type) < 1) return false;
        console.log("Hello here!");
        blockinlineView();

        fetch(this.#coreItem.detailHistPath + "?catId=" + parseInt(id) + "&catType=" + parseInt(type))
            .then(response => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.text();
            })
            .then(data => {
                document.getElementById('dvDetailHist').innerHTML = data;
                unblockinlineView();
            })
            .catch(error => {
                console.error('Error fetching detail history data:', error);
                unblockinlineView();
            });
    }


 displayTableListView() {
    blockMainView();

    fetch(this.#coreItem.servicePath + "list-view")
        .then(response => response.text())
        .then(data => {
            document.getElementById('dvTableListView').innerHTML = data;
            unblockMainView();
        })
        .catch(error => {
            console.error('Error fetching table list view data:', error);
            unblockMainView();
        });
}

 reloadTableListView(uPath) {
    blockMainView();

    fetch(uPath + "list-view")
        .then(response => response.text())
        .then(data => {
            document.getElementById('dvTableListView').innerHTML = data;
            unblockMainView();
        })
        .catch(error => {
            console.error('Error fetching reload table list view data:', error);
            unblockMainView();
        });
}

 reloadBulkListView(uPath) {
    blockMainView();

    fetch(uPath + "list-bulk")
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to fetch bulk list data");
            }
            return response.text();
        })
        .then(data => {
            document.getElementById('partialContainer').innerHTML = data;
            unblockMainView();
        })
        .catch(error => {
            console.error('Error reloading bulk list view:', error);
            unblockMainView();
        });
}


///Dont Trust this code

    switchDisplayView(calId) {
    if (calId === null || calId < 1) return false;
    const dvCardDisplay = document.getElementById('dvCardDisplay');
    const dvTableDisplay = document.getElementById('dvTableDisplay');
    const btnCardDisplayView = document.getElementById('btnCardDisplayView');
    const btnTableDisplayView = document.getElementById('btnTableDisplayView');

    dvCardDisplay.style.display = "none";
    dvTableDisplay.style.display = "none";
    btnCardDisplayView.classList.remove("tool-btn");
    btnTableDisplayView.classList.remove("tool-btn");

    switch (calId) {
        case 1:
            dvCardDisplay.style.display = "block";
            btnCardDisplayView.classList.add("tool-btn");
            break;
        case 2:
            dvTableDisplay.style.display = "block";
            btnTableDisplayView.classList.add("tool-btn");
            break;
        default:
    }
    }

    deleteItem(id) {
    if (id === "undefined" || id === null || id === "" || parseInt(id) < 1) {
        return ShowErrorAlert("Invalid Item Selection");
    }
    const coreItem = this.#coreItem;
    const succ = coreItem.deleteSuccessAlert;
    blockMainView();

    fetch(coreItem.servicePath + "delete-item?id=" + parseInt(id), {
        method: 'POST',
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to delete item");
            }
            return response.json();
        })
        .then(data => {
            if (!data.IsAuthenticated) {
                window.location.href = _signIn;
                return;
            }
            if (!data.IsSuccessful) {
                var retError = data.Error ? data.Error : "Unknown error occurred. Please try again later!";
                ShowErrorAlert(retError);
            } else {
                ShowSuccessPopupAlertReload(succ);
            }
            unblockMainView();
        })
        .catch(error => {
            console.error('Error deleting item:', error);
            unblockMainView();
        });
}



}
