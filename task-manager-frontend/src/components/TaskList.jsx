import React, { useState, useEffect, useRef } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { InputText } from 'primereact/inputtext';
import { Dropdown } from 'primereact/dropdown';
import { Calendar } from 'primereact/calendar';
import { Toast } from 'primereact/toast';
import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import 'primereact/resources/themes/saga-blue/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';
import './TaskList.css';
import axios from 'axios';

const API_URL = 'https://localhost:7032/api/Task'; // Replace with your API URL

const TaskList = () => {
    const [tasks, setTasks] = useState([]);
    const [taskDialogVisible, setTaskDialogVisible] = useState(false);
    const [task, setTask] = useState({
        title: '',
        description: '',
        status: 0,
        priority: 0,
        dueDate: null,
    });

    
    const [selectedTask, setSelectedTask] = useState(null);
    const toast = useRef(null); // Use useRef for Toast
    const [deleteConfirmationVisible, setDeleteConfirmationVisible] = useState(false); // State for delete confirmation
    const [taskToDelete, setTaskToDelete] = useState(null); // Store task to delete

    // Pagination state
    const [first, setFirst] = useState(0); // Tracks the first row index
    const [rows, setRows] = useState(5); // Number of rows per page (5 records per page)

    const taskStatusOptions = [
        { label: 'Todo', value: 0 },
        { label: 'In Progress', value: 1 },
        { label: 'Done', value: 2 },
    ];

    const taskPriorityOptions = [
        { label: 'Low', value: 0 },
        { label: 'Medium', value: 1 },
        { label: 'High', value: 2 },
    ];

    useEffect(() => {
        loadTasks();
    }, []);

    const loadTasks = async () => {
        try {
            const response = await axios.get(API_URL);
            setTasks(response.data);
        } catch (error) {
            console.error('Error fetching tasks:', error);
            showToast('error', 'Error', 'Failed to load tasks.');
        }
    };

    const openNewTaskDialog = () => {
        setTask({
            title: '',
            description: '',
            status: 0,
            priority: 0,
            dueDate: null,
        });
        setSelectedTask(null); // Clear selected task for creating new
        setTaskDialogVisible(true);
    };

    const hideTaskDialog = () => {
        setTaskDialogVisible(false);
    };

    const saveTask = async () => {
        try {
            if (selectedTask) {
                // Update existing task
                await axios.put(`${API_URL}/${selectedTask.id}`, task); // Assuming your API expects an ID for updates
                showToast('success', 'Success', 'Task updated successfully.');
            } else {
                // Create new task
                await axios.post(API_URL, task);
                showToast('success', 'Success', 'Task created successfully.');
            }
            loadTasks(); // Refresh task list
            hideTaskDialog();
        } catch (error) {
            console.error('Error saving task:', error);
            showToast('error', 'Error', 'Failed to save task.');
        }
    };

    const editTask = (task) => {
        setSelectedTask(task);
        setTask({ ...task, dueDate: new Date(task.dueDate) }); // Convert date string to Date object
        setTaskDialogVisible(true);
    };

    const confirmDeleteTask = (task) => {
        setTaskToDelete(task);
        setDeleteConfirmationVisible(true);
    };

    const deleteTask = async (id) => {
        try {
            await axios.delete(`${API_URL}/${id}`);
            loadTasks();
            showToast('success', 'Success', 'Task deleted successfully.');
        } catch (error) {
            console.error('Error deleting task:', error);
            showToast('error', 'Error', 'Failed to delete task.');
        } finally {
            setDeleteConfirmationVisible(false);
            setTaskToDelete(null);
        }
    };

    const onInputChange = (e, name) => {
        const val = (e.target && e.target.value) || '';
        setTask({ ...task, [name]: val });
    };

    const onDropdownChange = (e, name) => {
        setTask({ ...task, [name]: e.value });
    };

    const onDateChange = (e, name) => {
        setTask({ ...task, [name]: e.value });
    };

    const showToast = (severity, summary, detail) => {
        toast.current.show({ severity: severity, summary: summary, detail: detail, life: 3000 });
    };

    const actionBodyTemplate = (rowData) => {
        return (
            <React.Fragment>
                <Button
                    icon="pi pi-pencil"
                    className="p-button-rounded p-button-success mr-2"
                    onClick={() => editTask(rowData)}
                />
                <Button
                    icon="pi pi-trash"
                    className="p-button-rounded p-button-warning"
                    onClick={() => confirmDeleteTask(rowData)}
                />
            </React.Fragment>
        );
    };

    const taskDialogFooter = (
        <React.Fragment>
            <Button label="Cancel" icon="pi pi-times" className="p-button-text" onClick={hideTaskDialog} />
            <Button label="Save" icon="pi pi-check" className="p-button-text" onClick={saveTask} />
        </React.Fragment>
    );

    const deleteConfirmationFooter = (
        <React.Fragment>
            <Button
                label="No"
                icon="pi pi-times"
                className="p-button-text"
                onClick={() => setDeleteConfirmationVisible(false)}
            />
            <Button
                label="Yes"
                icon="pi pi-check"
                className="p-button-text"
                onClick={() => deleteTask(taskToDelete?.id)}
            />
        </React.Fragment>
    );

    
    const priorityBodyTemplate = (rowData) => {
        const priorityFromApi = rowData.priority; // Get the priority string from the API
        let priorityLabel = 'Unknown';
        let className = '';
        let icon = '';
    
        const priorityMap = {
            'high': { label: 'High', className: 'priority-high', icon: 'pi pi-fire' }, // Flame icon
            'medium': { label: 'Medium', className: 'priority-medium', icon: 'pi pi-bolt' }, // Bolt icon
            'low': { label: 'Low', className: 'priority-low', icon: 'pi pi-leaf' }, // Leaf icon
        };
    
        const matchingPriority = Object.keys(priorityMap).find(key => key.toLowerCase() === priorityFromApi.toLowerCase());
    
        if (matchingPriority) {
          priorityLabel = priorityMap[matchingPriority].label;
          className = priorityMap[matchingPriority].className;
          icon = priorityMap[matchingPriority].icon;
        }
    
    
        return (
            <span className={className}>
                <i className={icon + " p-mr-1"}></i>
                {priorityLabel.toLowerCase()} {/* Display label in lowercase */}
            </span>
        );
    };
    
    const statusBodyTemplate = (rowData) => {
        const statusFromApi = rowData.status; // Get the status string from the API
        let statusLabel = 'Unknown';
        let className = '';
        let icon = '';
    
        // Find the label in a case-insensitive way (and handle underscores)
        const matchingOption = taskStatusOptions.find(option => {
            const optionLabel = option.label.toLowerCase().replace(/ /g, '_'); // Normalize option label
            const apiStatus = statusFromApi.toLowerCase(); // Normalize API status
            return optionLabel === apiStatus;
        });
    
        if (matchingOption) {
            statusLabel = matchingOption.label;
        }
    
        const lowerCaseLabel = statusLabel.toLowerCase(); // For display
    
        switch (lowerCaseLabel) {
            case 'todo':
                className = 'status-todo';
                icon = 'pi pi-times-circle';
                break;
            case 'in progress':
                className = 'status-in-progress';
                icon = 'pi pi-spinner pi-spin';
                break;
            case 'done':
                className = 'status-done';
                icon = 'pi pi-check-circle';
                break;
            default:
                break;
        }
    
        return (
            <span className={className}>
                <i className={icon + " p-mr-1"}></i>
                {lowerCaseLabel}
            </span>
        );
    };

    return (
        <div className="task-list-container">
             <nav className="navbar">
                <div className="navbar-content">
                    <span className="navbar-logo">ITC TODO APP</span>
                    {/* Add more navigation items here if needed */}
                </div>
            </nav>

            <Toast ref={toast} />
            <ConfirmDialog />

            <div className="header">
                <h1>&nbsp;</h1>
                <Button 
                  icon="pi pi-plus" 
                  className="p-button-success add-button" 
                  label="Add New Task" 
                  onClick={openNewTaskDialog} 
                />
            </div>

            <div className="table-container">
                <DataTable
                    value={tasks}
                    paginator={true}
                    first={first}
                    rows={rows}
                    totalRecords={tasks.length}
                    onPage={(e) => setFirst(e.first)}
                    paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport"
                    currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
                    responsiveLayout="scroll"
                    className="task-table"
                >
                    <Column field="title" header="Title" sortable />
                    <Column field="description" header="Description" />
                     <Column field="status" header="Status" sortable body={statusBodyTemplate} />

                    <Column field="priority" header="Priority" sortable body={priorityBodyTemplate} />

                    <Column
                        field="dueDate"
                        header="Due Date"
                        sortable
                        body={(rowData) => {
                            const date = new Date(rowData.dueDate);
                            return date.toLocaleDateString(); // Format the date for display
                        }}
                    />
                    <Column body={actionBodyTemplate} header="Actions" />
                </DataTable>
            </div>

            <Dialog
                visible={taskDialogVisible}
                style={{ width: '50vw' }}
                header="Task Details"
                modal
                className="p-fluid task-dialog"
                footer={taskDialogFooter}
                onHide={hideTaskDialog}
            >
                <div className="p-field">
                    <label htmlFor="title">Title</label>
                    <InputText id="title" value={task.title} onChange={(e) => onInputChange(e, 'title')} required="true" />
                </div>
                <div className="p-field">
                    <label htmlFor="description">Description</label>
                    <InputText id="description" value={task.description} onChange={(e) => onInputChange(e, 'description')} />
                </div>
                <div className="p-field">
                    <label htmlFor="status">Status</label>
                    <Dropdown
                        id="status"
                        value={task.status}
                        options={taskStatusOptions}
                        onChange={(e) => onDropdownChange(e, 'status')}
                        placeholder="Select a Status"
                    />
                </div>
                <div className="p-field">
                    <label htmlFor="priority">Priority</label>
                    <Dropdown
                        id="priority"
                        value={task.priority}
                        options={taskPriorityOptions}
                        onChange={(e) => onDropdownChange(e, 'priority')}
                        placeholder="Select a Priority"
                    />
                </div>
                <div className="p-field">
                    <label htmlFor="dueDate">Due Date</label>
                    <Calendar id="dueDate" value={task.dueDate} onChange={(e) => onDateChange(e, 'dueDate')} dateFormat="mm/dd/yy" />
                </div>
            </Dialog>

            <Dialog
                visible={deleteConfirmationVisible}
                style={{ width: '450px' }}
                header="Confirm"
                modal
                footer={deleteConfirmationFooter}
                onHide={() => setDeleteConfirmationVisible(false)}
                className="delete-confirmation-dialog"
            >
                <div className="confirmation-content">
                    <i className="pi pi-exclamation-triangle p-mr-3" style={{ fontSize: '2rem' }} />
                    {taskToDelete && <span>Are you sure you want to delete <b>{taskToDelete.title}</b>?</span>}
                </div>
            </Dialog>
        </div>
    );

};

export default TaskList;