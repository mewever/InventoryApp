import { useState, useEffect } from 'react'
import axios from 'axios'
import Button from 'react-bootstrap/Button';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import InputGroup from 'react-bootstrap/InputGroup';
import Offcanvas from 'react-bootstrap/Offcanvas';
import Search from './assets/search.svg';
import PlusLg from './assets/plus-lg.svg';
import ArrowClockwise from './assets/arrow-clockwise.svg';
import './App.css'

function App() {
    const [itemList, setItemList] = useState<InventoryItem[]>([])
    const [selectedItem, setSelectedItem] = useState(new InventoryItem())
    const [showSelectedItem, setShowSelectedItem] = useState(false);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);

    // Manage visibility of the offcanvas area
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    // Set the item that is displayed in the main area and close the offcanvas area
    function setItem(item: InventoryItem) {
        setSelectedItem(item);
        setShowSelectedItem(true);
        setShow(false);
    }

    // Load the list of items from the API
    // This is outside of the initialization in useEffect() so it can be called from the refresh button
    function loadList() {
        setErrorMessage(null);
        axios.get<ApiResponseTyped<InventoryItem[]>>('https://localhost:7240/api/items/list')
            .then(response => {
                if (!response.data.isSuccessful) {
                    setErrorMessage(response.data.errorMessage);
                }
                else {
                    if (response.data.response == null) {
                        setItemList([]);
                    }
                    else {
                        setItemList(response.data.response);
                        // Update the item in the main area if necessary
                        if (showSelectedItem) {
                            // Use the API response rather than itemList to update the item in the main area.
                            // It seems like this code can be reached before itemList gets updated.
                            const idx = response.data.response.findIndex(i => i.id == selectedItem.id);
                            console.log('Found item ' + idx);
                            if (idx == -1) {
                                setShowSelectedItem(false);
                                return;
                            }
                            console.log('Setting item ' + response.data.response[idx].name);
                            setItem(response.data.response[idx]);
                        }
                    }
                }
            })
            .catch(err => console.log(err));
    }


    // Initialize the list of items for the ItemList component
    useEffect(() => {
        console.log("UseEffect()");
        loadList();
    }, []); // Empty array is key here to ensure this runs once on mount

    // Load the an empty item in the main area and close the offcanvas area (although it really shouldn't be open at this time)
    function addItem() {
        const newItem = new InventoryItem();
        newItem.name = 'New Inventory Item'
        setItem(newItem);
        setShow(false);
    };

    // Refresh by reloading the list of items and updating the item in the main area
    function refresh() {
        loadList();
        // loadList() handles updating of the item in the main area if necessary
    }

    // Handle a change in any field on the inventory item in the main area
    const handleInventoryItemChange = (e) => {
        const { name, value } = e.target;
        setSelectedItem({ ...selectedItem, [name]: value });
    };

    // Save changes to the item in the main area
    function saveInventoryItem() {
        setErrorMessage(null);
        if (selectedItem.id == 0) {
            // This is a new item, so add it
            axios.post<ApiResponseTyped<number>>('https://localhost:7240/api/items/add', selectedItem)
                .then(response => {
                    if (!response.data.isSuccessful) {
                        setErrorMessage(response.data.errorMessage);
                        return;
                    }
                    if (response.data.response == null) {
                        setErrorMessage('An unexpected error occurred.');
                        return;
                    }
                    // Set the ID number that was passed back from the api
                    const item = selectedItem;
                    item.id = response.data.response;
                    setSelectedItem(item);
                    // Add the item to the list
                    setItemList(itemList.concat(item));
                })
                .catch(err => console.log(err));
        }
        else {
            // This is an existing item, so update it
            axios.post<ApiResponse>('https://localhost:7240/api/items/update', selectedItem)
                .then(response => {
                    if (!response.data.isSuccessful) {
                        setErrorMessage(response.data.errorMessage);
                        return;
                    }
                    // Update the item in the list
                    const idx = itemList.findIndex(i => i.id == selectedItem.id);
                    setItemList(itemList.with(idx, selectedItem));
                })
                .catch(err => console.log(err));
        }
        // Set the flag to indicate no selected item should be shown.
        setSelectedItem(new InventoryItem())
        setShowSelectedItem(false);
    }

    function confirmDelete() {
        setShowDeleteConfirmation(true);
    }

    function cancelDelete() {
        setShowDeleteConfirmation(false);
    }

    // Delete the item in the main area
    function deleteInventoryItem() {
        setErrorMessage(null);
        const request: InventoryItemDeleteRequest = {
            id: selectedItem.id
        };
        axios.post<ApiResponse>('https://localhost:7240/api/items/delete', request)
            .then(response => {
                if (!response.data.isSuccessful) {
                    setErrorMessage(response.data.errorMessage);
                    return;
                }
                // Remove the item from the list
                setItemList(itemList.filter(item => item.id != selectedItem.id));
            })
            .catch(err => console.log(err));
        // Set the flag to indicate no selected item should be shown.
        setSelectedItem(new InventoryItem())
        setShowSelectedItem(false);
        setShowDeleteConfirmation(false);
    }

    return (
        <>
            <div>
                <ButtonToolbar>
                    <ButtonGroup>
                        <Button variant="primary" onClick={handleShow}>
                            <img src={Search} />
                        </Button>
                        <Button variant="primary" onClick={addItem}>
                            <img src={PlusLg} />
                        </Button>
                        <Button variant="primary" onClick={refresh}>
                            <img src={ArrowClockwise} />
                        </Button>
                    </ButtonGroup>
                </ButtonToolbar>

                <Offcanvas show={show} onHide={handleClose}>
                    <Offcanvas.Header closeButton>
                        <Offcanvas.Title>Items</Offcanvas.Title>
                    </Offcanvas.Header>
                    <Offcanvas.Body>
                        {//Map inventory item lines into the selection list
                            itemList.map(i =>
                                <div className="mb-1" key={i.id}>
                                    <Button type="button" variant="primary" onClick={() => setItem(i)}>
                                        [{i.productCode}] {i.name}
                                    </Button>
                                </div>
                            )}
                    </Offcanvas.Body>
                </Offcanvas>
            </div>

            {errorMessage != null ?
                // If there is an error message set, display it as a danger alert
                (
                    <>
                        <div className="alert alert-danger">
                            {errorMessage}
                        </div>
                    </>
                ) : (<></>)}

            {showSelectedItem ?
                // Item selected, so show it
                (
                    <>
                        <div>
                            <div className="text-center">
                                <h2>{selectedItem.name}</h2>
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Product Code</label>
                                <input type="text" className="form-control" name="productCode" value={selectedItem.productCode} onChange={handleInventoryItemChange} />
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Name</label>
                                <input type="text" className="form-control" name="name" value={selectedItem.name} onChange={handleInventoryItemChange} />
                            </div>

                            <div className="mb-3">
                                <label className="form-label">Quantity</label>
                                <input type="text" disabled readOnly className="form-control" value={selectedItem.quantity} />
                            </div>

                            <div className="mb-3">
                                {showDeleteConfirmation ?
                                    // This is the confirm button that actually deletes an item.
                                    // The confirmation button is first so that mindlessly double-clicking
                                    // should cause the cancel button to be clicked.
                                    (
                                        <>
                                            <InputGroup>
                                                <Button variant="danger" className="form-control" onClick={deleteInventoryItem}>Confirm</Button>
                                                <Button variant="primary" className="form-control" onClick={cancelDelete}>Cancel</Button>
                                            </InputGroup>
                                            <div>
                                                Confirm that you want to delete this record.<br />
                                            </div>
                                        </>
                                    ) :
                                    // This is the save and the initial delete button that causes a confirmation to appear.
                                    (
                                        <>
                                            <InputGroup>
                                                <Button variant="primary" className="form-control" onClick={saveInventoryItem}>Save</Button>
                                                <Button variant="danger" className="form-control" onClick={confirmDelete}>Delete</Button>
                                            </InputGroup>
                                        </>
                                    )}
                            </div>
                        </div>
                    </>
            ) :
                // No item selected, so show some instructions to get newbies started
                (
                        <>
                            <div className="text-center">
                                Press the search button to select a product.
                            </div>
                        </>
                )}
        </>
  )
}

class ApiResponse {
    isSuccessful: boolean = false;
    errorMessage: string | null = null;
}

class ApiResponseTyped<T> {
    isSuccessful: boolean = false;
    errorMessage: string | null = null;
    response: T | null = null;
}
class InventoryItem {
    id: number = 0;
    productCode: string = '';
    name: string = '';
    quantity: number = 0;
}

class InventoryItemDeleteRequest {
    id: number = 0;
}

export default App
