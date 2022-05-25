import React from 'react';
import { CellSet } from '../contracts';
import './HexCell.css';

function HexCell(props:
    {
        cellSet: CellSet
    }
) {
    return (
        <>
            <div className="left"></div>
            <div className="middle"></div>
            <div className="right"></div>
        </>
    );
}

export default HexCell;


