import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';

export default class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <h1>Welcome File Audit</h1>
            <p>You can see and audit the content of files retrieved by Downloader, some features include:</p>
            <ul>
                <li>View File Metadata</li>
                <li>View File Content</li>
                <li>Change File status</li>
            </ul>
        </div>;
    }
}
