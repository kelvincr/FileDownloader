import * as React from 'react';
import { fetch, addTask } from 'domain-task';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as FilesState from '../store/FileStorage';

// At runtime, Redux will merge together...
type FileProps =
FilesState.FileStorageState        // ... state we've requested from the Redux store
    & typeof FilesState.actionCreators      // ... plus action creators we've requested
    & RouteComponentProps<{ startIndex: string }>; // ... plus incoming routing parameters

class FetchData extends React.Component<FileProps, {}> {



    componentWillMount() {
        // This method runs when the component is first added to the page
        let startDateIndex = parseInt(this.props.match.params.startIndex) || 0;
        this.props.requestFilesAction(startDateIndex);
        this.updateFile = this.updateFile.bind(this);
    }

    componentWillReceiveProps(nextProps: FileProps) {
        // This method runs when incoming props (e.g., route params) change
        let startDateIndex = parseInt(nextProps.match.params.startIndex) || 0;
        this.props.requestFilesAction(startDateIndex);
    }

    updateFile(file: any, e : any){
        let startIndex = parseInt(this.props.match.params.startIndex) || 0;
        this.props.requestFileChangeStatusAction(file.id, e.target.value, file);
    }

    public render() {
        return <div>
            <h1>File Viewer</h1>
            <p>The following are the files stored in the server.</p>
            { this.renderFileTable() }
            { this.renderPagination() }
        </div>;
    }

    private renderFileTable() {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Server</th>
                    <th>Name</th>
                    <th>Size</th>
                    <th>Date</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
            {this.props.files.map(file =>
                <tr key={ file.id }>
                    <td>{ file.server }</td>
                    <td><button onClick={(e) => this.openFile(file.id, e)}><span className="glyphicon glyphicon-search"></span></button>{ file.name }</td>
                    <td>{ file.size }</td>
                    <td>{ file.date }</td>
                    <td>{this.renderStatus(file.status, file)}</td>
                </tr>
            )}
            </tbody>
        </table>;
    }

    private renderStatus(status : string, file : any){
        
        {if(status === "Approved" || status === "Rejected") 
        return status; 
        else return  this.renderDropDown(file) };
    }

    private renderDropDown(file : any){
        return <select  className="selectpicker" onChange={(e) => this.updateFile(file, e)}>
        <option>Ready to process</option>
        <option>Approved</option>
        <option>Rejected</option>
      </select>;
      
    }

    private renderPagination() {
        let prevStartDateIndex = (this.props.startIndex || 0) - 5;
        let nextStartDateIndex = (this.props.startIndex || 0) + 5;

        return <p className='clearfix text-center'>
        {prevStartDateIndex >= 0 ?
            <Link className='btn btn-default pull-left' to={ `/fetchdata/${ prevStartDateIndex }` }>Previous</Link>
            : <br/> }
            <Link className='btn btn-default pull-right' to={ `/fetchdata/${ nextStartDateIndex }` }>Next</Link>
            { this.props.isLoading ? <span>Loading...</span> : [] }
        </p>;
    }

    private openFile(id : number, e : any)
    {
        let fetchTask = fetch(`api/SampleData/file?id=${ id }`)
                .then(response => {
                     if(response.ok) {
                        return response.blob();
                }
                }).then(function(myBlob) { 
                  var objectURL = URL.createObjectURL(myBlob); 
                    window.open(objectURL);
                }).catch(function(error) {
                  console.log('There has been a problem with your fetch operation: ', error.message);
                });
            addTask(fetchTask);            
    }
}

export default connect(
    (state: ApplicationState) => state.files, // Selects which state properties are merged into the component's props
    FilesState.actionCreators                 // Selects which action creators are merged into the component's props
)(FetchData) as typeof FetchData;
