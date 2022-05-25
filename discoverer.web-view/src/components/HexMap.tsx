import React from 'react';
import './HexMap.css';
import HexCell from './HexCell';
import { CellSet } from '../contracts';

function HexMap(props:
    {
        cells: Array<Array<CellSet>>
    }) {
    return (
        <div className="HexMap">
            {props.cells.map((row, i) => 
                <div className="hex-row">
                    {row.map((cell, j) => 
                        <div className={(j % 2 === 1) ? 'hex even' : 'hex'}>
                            <HexCell key={j} cellSet={cell}/>
                        </div>)
                    }
                </div>)
            }
        </div>
    );
}

export default HexMap;
