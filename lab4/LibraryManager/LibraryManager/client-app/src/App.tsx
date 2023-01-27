import React from "react";
import "./App.css";
import NavigationBar from "./components/NavigationBar";
import { CustomRouter } from "./utils/CustomRouter";
import customHistory from "./history";
import { AppRoutes } from "./AppRoutes";

function App() {
    return (
        <div className="App">
            <CustomRouter history={customHistory}>
                <NavigationBar />
                <AppRoutes />
            </CustomRouter>
        </div>
    );
}

export default App;
