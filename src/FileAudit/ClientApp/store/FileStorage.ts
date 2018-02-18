import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface FileStorageState {
    isLoading: boolean;
    startIndex?: number;
    files: File[];
}

export interface File {
    id : number;
    server: string;
    name: string;
    size: number;
    date: string;
    status: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface UpdateFileStatusAction {
    type: 'UPDATE_FILE';
    id: number;
    status : string;
    file : File;
}

interface RequestFilesAction {
    type: 'REQUEST_FILES';
    startIndex: number;
}

interface ReceiveFilesAction {
    type: 'RECEIVE_FILES';
    startIndex: number;
    files: File[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestFilesAction | ReceiveFilesAction | UpdateFileStatusAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestFilesAction: (startIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        if (startIndex !== getState().files.startIndex) {
            let fetchTask = fetch(`api/SampleData/Files?startIndex=${ startIndex }`)
                .then(response => response.json() as Promise<File[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_FILES', startIndex: startIndex, files: data });
                });

            addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
            dispatch({ type: 'REQUEST_FILES', startIndex: startIndex });
        }
    },
    requestFileChangeStatusAction: (id: number, status: string, file : File) : AppThunkAction<KnownAction> => (dispatch, getState) => {
        if(status !== "Ready to process"){
            let fetchTask = fetch(`api/SampleData/Update?id=${id}`, {
                method: 'post',
                headers: {'Content-Type':'application/json'},
                body: JSON.stringify(status)
              } );
              addTask(fetchTask);
              dispatch({ type: 'UPDATE_FILE', id: id, status : status, file: file });
        }
    }

};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: FileStorageState = { files: [], isLoading: false };

export const reducer: Reducer<FileStorageState> = (state: FileStorageState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_FILES':
            return {
                startIndex: action.startIndex,
                files: state.files,
                isLoading: true
            };
        case 'RECEIVE_FILES':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.startIndex === state.startIndex) {
                return {
                    startIndex: action.startIndex,
                    files: action.files,
                    isLoading: false
                };
            }
            break;
        case 'UPDATE_FILE':
            let newList = state.files.slice();
            let objIndex = newList.findIndex((obj => obj.id == action.id));
            newList[objIndex].status = action.status;
            return {
                startIndex : state.startIndex,
                files : newList,
                isLoading : state.isLoading
            }
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};
